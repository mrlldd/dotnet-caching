using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Tests.Caches.TestUtilities
{
    public class OnlyMemoryCache<T> : TestCache<T>
    {
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Disabled;
    }
}