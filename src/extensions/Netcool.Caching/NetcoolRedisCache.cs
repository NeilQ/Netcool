using System.Reflection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Netcool.Caching;

public class NetcoolRedisCache : RedisCache, INetcoolDistributedCache
{
    private readonly ISerializer _serializer;

    protected static readonly string DataKey;

    private static readonly FieldInfo RedisDatabaseField;
    private static readonly MethodInfo ConnectMethod;
    private static readonly MethodInfo ConnectAsyncMethod;

    protected IDatabase RedisDatabase => GetRedisDatabase();
    private IDatabase _redisDatabase;

    private const string IncrWithLimitsScript = @"
         local currVal = redis.call('HINCRBY', KEYS[1], 'data', ARGV[1])
         if currVal > tonumber(ARGV[2]) then 
             redis.call('HSET', KEYS[1], 'data', ARGV[2]) 
             return ARGV[2]
         end
         return currVal";

    private const string DecrWithLimitsScript = @"
         local currVal = redis.call('HINCRBY', KEYS[1], 'data', ARGV[1])
         if currVal < tonumber(ARGV[2]) then 
            redis.call('HSET', KEYS[1], 'data', ARGV[2]) 
            return ARGV[2]
         end
         return currVal";

    protected string Instance { get; }

    static NetcoolRedisCache()
    {
        var type = typeof(RedisCache);

        RedisDatabaseField = type.GetField("_cache", BindingFlags.Instance | BindingFlags.NonPublic);

        ConnectMethod = type.GetMethod("Connect", BindingFlags.Instance | BindingFlags.NonPublic);

        ConnectAsyncMethod = type.GetMethod("ConnectAsync", BindingFlags.Instance | BindingFlags.NonPublic);

        DataKey = type.GetField("DataKey", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null).ToString();
    }

    public NetcoolRedisCache(IOptions<NetcoolRedisCacheOptions> optionsAccessor) : base(optionsAccessor)
    {
        _serializer = optionsAccessor.Value.ObjectSerializer ?? new SystemTextJsonSerializer();
        Instance = optionsAccessor.Value.InstanceName ?? string.Empty;
    }

    public T GetObject<T>(string key) where T : class
    {
        var value = Get(key);
        if (value == null || value.Length == 0) return null;
        return _serializer.Deserialize<T>(value);
    }

    public async Task<T> GetObjectAsync<T>(string key, CancellationToken token = default) where T : class
    {
        var value = await GetAsync(key, token);
        if (value == null || value.Length == 0) return null;
        return _serializer.Deserialize<T>(value);
    }

    public void SetObject<T>(string key, T value, DistributedCacheEntryOptions options) where T : class
    {
        var bytes = _serializer.Serialize(value);
        Set(key, bytes, options);
    }

    public async Task SetObjectAsync<T>(string key, T value, DistributedCacheEntryOptions options,
        CancellationToken token = default) where T : class
    {
        var bytes = _serializer.Serialize(value);
        await SetAsync(key, bytes, options, token);
    }

    public long Increase(string key, long byValue = 1L, long? maxValue = null, bool fireAndForget = false)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        Connect();
        long value;
        if (maxValue == null)
        {
            value = RedisDatabase.HashIncrement(Instance + key, DataKey, byValue);
        }
        else
        {
            var res = RedisDatabase.ScriptEvaluate(IncrWithLimitsScript, new RedisKey[] { Instance + key },
                new RedisValue[] { byValue, maxValue.Value },
                GetCommandFlag(fireAndForget));
            value = res == null ? default : (long)res;
        }

        return value;
    }

    public async Task<long> IncreaseAsync(string key, long byValue = 1L, long? maxValue = null,
        bool fireAndForget = false, CancellationToken token = default)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        token.ThrowIfCancellationRequested();
        await ConnectAsync(token).ConfigureAwait(false);
        long value;
        if (maxValue == null)
        {
            value = await RedisDatabase.HashIncrementAsync(Instance + key, DataKey, byValue);
        }
        else
        {
            var res = await RedisDatabase.ScriptEvaluateAsync(IncrWithLimitsScript,
                new RedisKey[] { Instance + key },
                new RedisValue[] { byValue, maxValue.Value },
                GetCommandFlag(fireAndForget));
            value = res == null ? default : (long)res;
        }

        return value;
    }

    public long Decrease(string key, long byValue = 1L, long? minValue = null, bool fireAndForget = false)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        Connect();
        long value;
        if (minValue == null)
        {
            value = RedisDatabase.HashDecrement(Instance + key, DataKey, byValue);
        }
        else
        {
            var res = RedisDatabase.ScriptEvaluate(DecrWithLimitsScript,
                new RedisKey[] { Instance + key },
                new RedisValue[] { -byValue, minValue.Value },
                GetCommandFlag(fireAndForget));
            value = res == null ? default : (long)res;
        }

        return value;
    }

    public async Task<long> DecreaseAsync(string key, long byValue = 1L, long? minValue = null,
        bool fireAndForget = false, CancellationToken token = default)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        token.ThrowIfCancellationRequested();
        await ConnectAsync(token).ConfigureAwait(false);
        long value;
        if (minValue == null)
        {
            value = await RedisDatabase.HashDecrementAsync(Instance + key, DataKey, byValue);
        }
        else
        {
            var res = await RedisDatabase.ScriptEvaluateAsync(DecrWithLimitsScript,
                new RedisKey[] { Instance + key },
                new RedisValue[] { -byValue, minValue.Value },
                GetCommandFlag(fireAndForget));
            value = res == null ? default : (long)res;
        }

        return value;
    }

    private CommandFlags GetCommandFlag(bool fireAndForget)
    {
        return fireAndForget ? CommandFlags.FireAndForget : CommandFlags.None;
    }

    protected virtual void Connect()
    {
        if (GetRedisDatabase() != null) return;

        ConnectMethod.Invoke(this, Array.Empty<object>());
    }

    protected virtual Task ConnectAsync(CancellationToken token = default)
    {
        if (GetRedisDatabase() != null) return Task.CompletedTask;

        return (Task)ConnectAsyncMethod.Invoke(this, new object[] { token });
    }

    private IDatabase GetRedisDatabase()
    {
        return _redisDatabase ??= RedisDatabaseField.GetValue(this) as IDatabase;
    }
}
