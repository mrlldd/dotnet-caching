using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Decoration.Internal.Logging
{
    internal abstract class LoggingCacheStoreDecorator : ICacheStoreDecorator
    {
        protected const string MemoryStoreLogPrefix = "Memory Cache";

        protected const string DistributedStoreLogPrefix = "Distributed Cache";
        
        public abstract IMemoryCacheStore Decorate(IMemoryCacheStore memoryCacheStore);
        public abstract IDistributedCacheStore Decorate(IDistributedCacheStore distributedCacheStore);
        public abstract int Order { get; }
    }
}