﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Netcool.Caching;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNetcoolRedisCache(this IServiceCollection services,
        Action<NetcoolRedisCacheOptions> setupAction)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

        services.AddOptions();
        services.Configure(setupAction);
        services.TryAddSingleton<INetcoolDistributedCache, NetcoolRedisCache>();
        return services;
    }

    public static IServiceCollection AddNetcoolDistributedMemoryCache(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddOptions();
        services.TryAdd(ServiceDescriptor.Singleton<INetcoolDistributedCache, NetcoolMemoryDistributedCache>());

        return services;
    }

    public static IServiceCollection AddNetcoolDistributedMemoryCache(this IServiceCollection services,
        Action<NetcoolMemoryDistributedCacheOptions> setupAction)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddNetcoolDistributedMemoryCache();
        services.Configure(setupAction);

        return services;
    }
}
