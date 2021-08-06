using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Extensions.DependencyInjection;

namespace mrlldd.Caching.Stores.Decoration.Internal.Logging
{
    internal class LoggingDistributedCachingStore : LoggingCachingStore<IDistributedCachingStore, DistributedCacheEntryOptions>, IDistributedCachingStore
    {
        public LoggingDistributedCachingStore(IDistributedCachingStore sourceCachingStore,
            ILogger<LoggingCachingStore<IDistributedCachingStore, DistributedCacheEntryOptions>> logger,
            ICachingLoggingOptions cachingLoggingOptions,
            string storeLogPrefix) 
            : base(sourceCachingStore, logger, cachingLoggingOptions, storeLogPrefix)
        {
        }
    }
}