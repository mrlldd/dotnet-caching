using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Performance
{
    internal class PerformanceLoggingDistributedCacheStore : PerformanceLoggingCacheStore<IDistributedCacheStore, DistributedCacheEntryOptions>, IDistributedCacheStore
    {
        public PerformanceLoggingDistributedCacheStore(IDistributedCacheStore sourceCacheStore,
            ILogger<PerformanceLoggingCacheStore<IDistributedCacheStore, DistributedCacheEntryOptions>> logger,
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