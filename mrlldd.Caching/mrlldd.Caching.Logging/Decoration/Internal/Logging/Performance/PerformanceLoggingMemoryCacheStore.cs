using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Performance
{
    internal class PerformanceLoggingMemoryCacheStore : PerformanceLoggingCacheStore<IMemoryCacheStore, MemoryCacheEntryOptions>, IMemoryCacheStore
    {
        public PerformanceLoggingMemoryCacheStore(IMemoryCacheStore sourceCacheStore,
            ILogger<PerformanceLoggingCacheStore<IMemoryCacheStore, MemoryCacheEntryOptions>> logger, 
            ICachingPerformanceLoggingOptions loggingOptions, 
            string storeLogPrefix) 
            : base(sourceCacheStore, 
                logger, 
                loggingOptions,
                storeLogPrefix)
        {
        }
    }
}