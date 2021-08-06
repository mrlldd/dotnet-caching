using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Tests.Caches.TestUtilities
{
    public class BubbleCache<T> : TestCache<T>
    {
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Disabled;

        protected override CachingOptions MemoryCacheOptions => CachingOptions.Disabled;
    }
}