using Microsoft.Extensions.Logging;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Actions
{
    internal sealed class ActionsLoggingCacheStoreDecorator<TFlag> : LoggingCacheStoreDecorator<TFlag> where TFlag : CachingFlag
    {
        private readonly ICachingActionsLoggingOptions options;
        private readonly ILogger<ICacheStore<TFlag>> logger;

        public ActionsLoggingCacheStoreDecorator(ILogger<ICacheStore<TFlag>> logger,
            ICachingActionsLoggingOptions options)
        {
            this.logger = logger;
            this.options = options;
        }

        public override ICacheStore<TFlag> Decorate(ICacheStore<TFlag> cacheStore)
            => new ActionsLoggingCacheStore<TFlag>(cacheStore, logger, options, LogPrefix);

        public override int Order => int.MaxValue;
    }
}