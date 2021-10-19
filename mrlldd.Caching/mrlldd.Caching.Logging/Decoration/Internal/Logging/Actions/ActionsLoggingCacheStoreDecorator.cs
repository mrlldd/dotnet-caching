using Microsoft.Extensions.Logging;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Actions
{
    internal sealed class ActionsLoggingCacheStoreDecorator<TFlag> : LoggingCacheStoreDecorator<TFlag>
        where TFlag : CachingFlag
    {
        private readonly ILogger<ICacheStore<TFlag>> logger;
        private readonly ICachingActionsLoggingOptions options;

        public ActionsLoggingCacheStoreDecorator(ILogger<ICacheStore<TFlag>> logger,
            ICachingActionsLoggingOptions options)
        {
            this.logger = logger;
            this.options = options;
        }

        public override int Order => int.MaxValue;

        public override ICacheStore<TFlag> Decorate(ICacheStore<TFlag> cacheStore)
        {
            return new ActionsLoggingCacheStore<TFlag>(cacheStore, logger, options, LogPrefix);
        }
    }
}