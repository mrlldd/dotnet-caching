using Microsoft.Extensions.Logging;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Stores.Internal;

namespace mrlldd.Caching.Stores.Decoration.Internal.Logging
{
    internal class LoggingCachingStoreDecorator : ICachingStoreDecorator
    {
        private readonly ILogger<LoggingMemoryCachingStore> memoryCacheLogger;
        private readonly ILogger<LoggingDistributedCachingStore> distributedStoreLogger;
        private readonly ICachingLoggingOptions options;

        public LoggingCachingStoreDecorator(ILogger<LoggingMemoryCachingStore> memoryCacheLogger,
            ILogger<LoggingDistributedCachingStore> distributedStoreLogger,
            ICachingLoggingOptions options)
        {
            this.memoryCacheLogger = memoryCacheLogger;
            this.distributedStoreLogger = distributedStoreLogger;
            this.options = options;
        }

        public IMemoryCachingStore Decorate(IMemoryCachingStore memoryCachingStore) 
            => new LoggingMemoryCachingStore(memoryCachingStore, memoryCacheLogger, options, nameof(MemoryCachingStore));

        public IDistributedCachingStore Decorate(IDistributedCachingStore distributedCachingStore) 
            => new LoggingDistributedCachingStore(distributedCachingStore, distributedStoreLogger, options, nameof(DistributedCachingStore));

        public int Order => int.MinValue;
    }
}