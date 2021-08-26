namespace mrlldd.Caching.Caches.Internal
{
    internal class NullDefaultCacheOptions : ICacheOptions
    {
        public CachingOptions MemoryCacheOptions => CachingOptions.Disabled; 
        public CachingOptions DistributedCacheOptions => CachingOptions.Disabled;
    }
}