using Microsoft.Extensions.Caching.Memory;

namespace Netcool.Caching;

public class NetcoolMemoryDistributedCacheOptions : MemoryDistributedCacheOptions
{
    public ISerializer ObjectSerializer { get; set; }
    
    public NetcoolMemoryDistributedCacheOptions()
    {
        SizeLimit = null;
    }
}
