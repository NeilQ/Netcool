using Microsoft.Extensions.Caching.Distributed;

namespace Netcool.Caching;

public interface INetcoolDistributedCache : IDistributedCache
{
    T GetObject<T>(string key) where T : class;

    Task<T> GetObjectAsync<T>(string key, CancellationToken token = default) where T : class;

    void SetObject<T>(string key, T value, DistributedCacheEntryOptions options)
        where T : class;

    Task SetObjectAsync<T>(string key, T value, DistributedCacheEntryOptions options,
        CancellationToken token = default)
        where T : class;

    long Increase(string key, long byValue = 1L, long? maxValue = null, bool fireAndForget = false);

    Task<long> IncreaseAsync(string key, long byValue = 1L, long? maxValue = null, bool fireAndForget = false,
        CancellationToken token = default);

    long Decrease(string key, long byValue = 1L, long? minValue = null, bool fireAndForget = false);

    Task<long> DecreaseAsync(string key, long byValue = 1L, long? minValue = null, bool fireAndForget = false,
        CancellationToken token = default);
}
