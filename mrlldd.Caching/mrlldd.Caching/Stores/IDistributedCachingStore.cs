using Microsoft.Extensions.Caching.Distributed;

namespace mrlldd.Caching.Stores
{
    public interface IDistributedCachingStore : ICachingStore<DistributedCacheEntryOptions>
    {
        
    }
}