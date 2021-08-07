using Microsoft.Extensions.Logging;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Performance
{
    internal sealed class PerformanceLoggingCacheStoreDecorator : LoggingCacheStoreDecorator 
    {
        private readonly ILogger<PerformanceLoggingMemoryCacheStore> memoryCacheLogger;
        private readonly ILogger<PerformanceLoggingDistributedCacheStore> distributedStoreLogger;
        private readonly ICachingPerformanceLoggingOptions options;

        public PerformanceLoggingCacheStoreDecorator(ILogger<PerformanceLoggingMemoryCacheStore> memoryCacheLogger,
            ILogger<PerformanceLoggingDistributedCacheStore> distributedStoreLogger,
            ICachingPerformanceLoggingOptions options)
        {
            this.memoryCacheLogger = memoryCacheLogger;
            this.distributedStoreLogger = distributedStoreLogger;
            this.options = options;
        }

        public override IMemoryCacheStore Decorate(IMemoryCacheStore memoryCacheStore)
            => new PerformanceLoggingMemoryCacheStore(memoryCacheStore, memoryCacheLogger, options, MemoryStoreLogPrefix);

        public override IDistributedCacheStore Decorate(IDistributedCacheStore distributedCacheStore)
            => new PerformanceLoggingDistributedCacheStore(distributedCacheStore, distributedStoreLogger, options,
                DistributedStoreLogPrefix);

        public override int Order => int.MinValue;
    }
}