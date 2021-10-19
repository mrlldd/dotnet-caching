using Microsoft.Extensions.Logging;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Performance
{
    internal sealed class PerformanceLoggingCacheStoreDecorator<TFlag> : LoggingCacheStoreDecorator<TFlag>
        where TFlag : CachingFlag
    {
        private readonly ILogger<ICacheStore<TFlag>> logger;
        private readonly ICachingPerformanceLoggingOptions options;

        public PerformanceLoggingCacheStoreDecorator(ILogger<ICacheStore<TFlag>> logger,
            ICachingPerformanceLoggingOptions options)
        {
            this.logger = logger;
            this.options = options;
        }

        public override int Order => int.MinValue;

        public override ICacheStore<TFlag> Decorate(ICacheStore<TFlag> cacheStore)
        {
            return new PerformanceLoggingCacheStore<TFlag>(cacheStore, logger, options, LogPrefix);
        }
    }
}