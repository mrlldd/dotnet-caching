using Microsoft.Extensions.Caching.Distributed;

namespace mrlldd.Caching.Stores
{
    /// <summary>
    /// The interface that represents distributed caching store.
    /// </summary>
    public interface IDistributedCacheStore : ICacheStore<DistributedCacheEntryOptions>
    {
        
    }
}