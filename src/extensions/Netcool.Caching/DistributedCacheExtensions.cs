using Microsoft.Extensions.Caching.Distributed;

namespace Netcool.Caching;

public static class DistributedCacheExtensions
{
    public static void SetObject<T>(this INetcoolDistributedCache cache, string key, T value) where T : class
    {
        cache.SetObject(key, value, new DistributedCacheEntryOptions());
    }

    public static Task SetObjectAsync<T>(this INetcoolDistributedCache cache, string key, T value,
        CancellationToken token = default)
        where T : class
    {
        return cache.SetObjectAsync(key, value, new DistributedCacheEntryOptions(), token);
    }
}
