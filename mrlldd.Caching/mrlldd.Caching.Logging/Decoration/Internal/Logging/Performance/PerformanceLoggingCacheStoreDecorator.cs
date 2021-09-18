using Microsoft.Extensions.Logging;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Performance
{
    internal sealed class PerformanceLoggingCacheStoreDecorator<TFlag> : LoggingCacheStoreDecorator<TFlag>
        where TFlag : CachingFlag
    {
        private readonly ICachingPerformanceLoggingOptions options;
        private readonly ILogger<ICacheStore<TFlag>> logger;

        public PerformanceLoggingCacheStoreDecorator(ILogger<ICacheStore<TFlag>> logger,
            ICachingPerformanceLoggingOptions options)
        {
            this.logger = logger;
            this.options = options;
        }

        public override ICacheStore<TFlag> Decorate(ICacheStore<TFlag> cacheStore)
            => new PerformanceLoggingCacheStore<TFlag>(cacheStore, logger, options, LogPrefix);

        public override int Order => int.MinValue;
    }
}