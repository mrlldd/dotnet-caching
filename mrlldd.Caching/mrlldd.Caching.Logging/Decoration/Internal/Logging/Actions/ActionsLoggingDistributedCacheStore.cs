using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Actions
{
    internal class ActionsLoggingDistributedCacheStore : ActionsLoggingCacheStore<IDistributedCacheStore, DistributedCacheEntryOptions>, IDistributedCacheStore
    {
        public ActionsLoggingDistributedCacheStore(IDistributedCacheStore sourceCacheStore,
            ILogger<ActionsLoggingCacheStore<IDistributedCacheStore, DistributedCacheEntryOptions>> logger,
            ICachingActionsLoggingOptions cachingLoggingOptions,
            string storeLogPrefix) 
            : base(sourceCacheStore, 
                logger,
                cachingLoggingOptions,
                storeLogPrefix)
        {
        }
    }
}