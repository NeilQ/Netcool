using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Netcool.Caching;

public class NetcoolRedisCacheOptions : RedisCacheOptions
{
    public ISerializer ObjectSerializer { get; set; }
}
