using Microsoft.Extensions.Caching.Memory;

namespace mrlldd.Caching.Stores
{
    public interface IMemoryCachingStore : ICachingStore<MemoryCacheEntryOptions>
    {
        
    }
}