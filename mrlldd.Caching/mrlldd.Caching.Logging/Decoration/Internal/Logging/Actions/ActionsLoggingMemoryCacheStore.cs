using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Actions
{
    internal class ActionsLoggingMemoryCacheStore : ActionsLoggingCacheStore<IMemoryCacheStore, MemoryCacheEntryOptions>, IMemoryCacheStore
    {
        public ActionsLoggingMemoryCacheStore(IMemoryCacheStore sourceCacheStore,
            ILogger<ActionsLoggingCacheStore<IMemoryCacheStore, MemoryCacheEntryOptions>> logger,
            ICachingActionsLoggingOptions loggingOptions,
            string storeLogPrefix)
            : base(sourceCacheStore, logger, loggingOptions, storeLogPrefix)
        {
        }
    }
}