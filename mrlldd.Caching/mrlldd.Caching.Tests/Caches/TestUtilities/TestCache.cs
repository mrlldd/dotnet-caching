using System;
using System.Collections.Generic;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Tests.Caches.TestUtilities
{
    public class TestCache<T> : Cache<T>
    {
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Enabled(TimeSpan.MaxValue);
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Enabled(TimeSpan.MaxValue);

        public const string GlobalCacheKey = "testcache";

        public const string CacheKeyPrefix = "test";
        protected override string CacheKey => GlobalCacheKey;
        protected override string DefaultKeySuffix => typeof(T).Name;
    }
}