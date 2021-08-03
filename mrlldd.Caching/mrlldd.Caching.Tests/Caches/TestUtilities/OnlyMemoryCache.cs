using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Tests.Caches.TestUtilities
{
    public class OnlyMemoryCache : TestCache<TestUnit>
    {
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Disabled;
    }
}