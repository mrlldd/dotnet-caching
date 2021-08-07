using Microsoft.Extensions.Caching.Memory;

namespace mrlldd.Caching.Stores
{
    /// <summary>
    /// The interface that represents memory caching store.
    /// </summary>
    public interface IMemoryCacheStore : ICacheStore<MemoryCacheEntryOptions>
    {
        
    }
}