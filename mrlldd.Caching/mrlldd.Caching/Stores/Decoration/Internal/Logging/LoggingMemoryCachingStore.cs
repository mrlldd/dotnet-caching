using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Extensions.DependencyInjection;

namespace mrlldd.Caching.Stores.Decoration.Internal.Logging
{
    internal class LoggingMemoryCachingStore : LoggingCachingStore<IMemoryCachingStore, MemoryCacheEntryOptions>, IMemoryCachingStore
    {
        public LoggingMemoryCachingStore(IMemoryCachingStore sourceCachingStore,
            ILogger<LoggingCachingStore<IMemoryCachingStore, MemoryCacheEntryOptions>> logger,
            ICachingLoggingOptions loggingOptions,
            string storeLogPrefix)
            : base(sourceCachingStore, logger, loggingOptions, storeLogPrefix)
        {
        }
    }
}