using Microsoft.Extensions.Logging;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Decoration.Internal.Logging.Actions
{
    internal sealed class ActionsLoggingCacheStoreDecorator : LoggingCacheStoreDecorator
    {
        private readonly ILogger<ActionsLoggingMemoryCacheStore> memoryCacheLogger;
        private readonly ILogger<ActionsLoggingDistributedCacheStore> distributedStoreLogger;
        private readonly ICachingActionsLoggingOptions options;
        public ActionsLoggingCacheStoreDecorator(ILogger<ActionsLoggingMemoryCacheStore> memoryCacheLogger,
            ILogger<ActionsLoggingDistributedCacheStore> distributedStoreLogger,
            ICachingActionsLoggingOptions options)
        {
            this.memoryCacheLogger = memoryCacheLogger;
            this.distributedStoreLogger = distributedStoreLogger;
            this.options = options;
        }

        public override IMemoryCacheStore Decorate(IMemoryCacheStore memoryCacheStore) 
            => new ActionsLoggingMemoryCacheStore(memoryCacheStore, memoryCacheLogger, options, MemoryStoreLogPrefix);

        public override IDistributedCacheStore Decorate(IDistributedCacheStore distributedCacheStore) 
            => new ActionsLoggingDistributedCacheStore(distributedCacheStore, distributedStoreLogger, options, DistributedStoreLogPrefix);

        public override int Order => int.MaxValue;
    }
}