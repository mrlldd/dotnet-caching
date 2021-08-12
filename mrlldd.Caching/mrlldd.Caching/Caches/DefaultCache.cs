namespace mrlldd.Caching.Caches
{
    internal sealed class DefaultCache<T> : Cache<T>
    {
        protected override CachingOptions MemoryCacheOptions { get; }
        protected override CachingOptions DistributedCacheOptions { get; }
        protected override string CacheKey => typeof(T).Name;
        
        public DefaultCache(ICacheOptions cacheOptions)
        {
            MemoryCacheOptions = cacheOptions.MemoryCacheOptions;
            DistributedCacheOptions = cacheOptions.DistributedCacheOptions;
        }
    }
}