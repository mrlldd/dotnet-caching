using System;
using Functional.Result;
using Moq;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.Caches
{
    public class EnabledCachingCacheStoreCallTests : CacheStoreCallTestFixture
    {
        protected override CachingOptions CachingOptions { get; } =
            CachingOptions.Enabled(TimeSpan.FromMilliseconds(1));

        protected override Func<Times> Hits => Times.Once;
        protected override Result Result => Result.Success;

        protected override Result<VoidUnit?> ResultFactory(VoidUnit unit)
        {
            return unit;
        }
    }
}