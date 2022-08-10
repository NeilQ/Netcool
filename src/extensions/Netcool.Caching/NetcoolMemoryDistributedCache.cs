using System.Reflection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Netcool.Caching;

public class NetcoolMemoryDistributedCache : MemoryDistributedCache, INetcoolDistributedCache
{
    private MemoryCache _memCache;
    protected MemoryCache MemCache => GetMemoryCache();

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    private static readonly FieldInfo MemCacheField;

    private readonly ISerializer _cacheSerializer;

    private MemoryCache GetMemoryCache()
    {
        return _memCache ??= MemCacheField.GetValue(this) as MemoryCache;
    }

    static NetcoolMemoryDistributedCache()
    {
        var type = typeof(MemoryDistributedCache);
        MemCacheField = type.GetField("_memCache", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    public NetcoolMemoryDistributedCache(IOptions<NetcoolMemoryDistributedCacheOptions> optionsAccessor) : base(
        optionsAccessor)
    {
        _cacheSerializer = optionsAccessor.Value.ObjectSerializer ?? new SystemTextJsonSerializer();
    }

    public NetcoolMemoryDistributedCache(IOptions<NetcoolMemoryDistributedCacheOptions> optionsAccessor,
        ILoggerFactory loggerFactory) : base(optionsAccessor, loggerFactory)
    {
        _cacheSerializer = optionsAccessor.Value.ObjectSerializer ?? new SystemTextJsonSerializer();
    }

    public T GetObject<T>(string key) where T : class
    {
        var bytes = Get(key);
        return _cacheSerializer.Deserialize<T>(bytes);
    }

    public Task<T> GetObjectAsync<T>(string key, CancellationToken token = default) where T : class
    {
        return Task.FromResult(GetObject<T>(key));
    }

    public void SetObject<T>(string key, T value, DistributedCacheEntryOptions options)
        where T : class
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (options == null) throw new ArgumentNullException(nameof(options));

        var bytes = _cacheSerializer.Serialize(value);
        Set(key, bytes, options);
    }

    public Task SetObjectAsync<T>(string key, T value, DistributedCacheEntryOptions options,
        CancellationToken token = default) where T : class
    {
        SetObject(key, value, options);
        return Task.CompletedTask;
    }

    public long Increase(string key, long byValue = 1, long? maxValue = null, bool fireAndForget = false)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        long val;
        _semaphoreSlim.Wait();
        try
        {
            val = DoInternalIncrease(key, byValue, maxValue);
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return val;
    }

    public async Task<long> IncreaseAsync(string key, long byValue = 1, long? maxValue = null,
        bool fireAndForget = false, CancellationToken token = default)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        long val;
        await _semaphoreSlim.WaitAsync(token);
        try
        {
            val = DoInternalIncrease(key, byValue, maxValue);
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return val;
    }

    private long DoInternalIncrease(string key, long byValue, long? maxValue)
    {
        long val;
        var valStr = this.GetString(key);
        if (long.TryParse(valStr, out var value))
            val = value + byValue;
        else
            val = byValue;

        if (val > maxValue) val = maxValue.Value;

        this.SetString(key, val.ToString());
        return val;
    }


    public long Decrease(string key, long byValue = 1, long? minValue = null, bool fireAndForget = false)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        long val;
        _semaphoreSlim.Wait();
        try
        {
            val = DoInternalDecrease(key, byValue, minValue);
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return val;
    }


    public async Task<long> DecreaseAsync(string key, long byValue = 1, long? minValue = null,
        bool fireAndForget = false, CancellationToken token = default)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        long val;
        await _semaphoreSlim.WaitAsync(token);
        try
        {
            val = DoInternalDecrease(key, byValue, minValue);
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return val;
    }

    private long DoInternalDecrease(string key, long byValue, long? minValue)
    {
        long val;
        var valStr = this.GetString(key);
        if (long.TryParse(valStr, out var value))
            val = value - byValue;
        else
            val = byValue;

        if (val < minValue) val = minValue.Value;

        this.SetString(key, val.ToString());
        return val;
    }
}
