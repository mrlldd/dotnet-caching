using System;
using Functional.Result;
using Moq;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.Loaders
{
    public class DisabledCachingCacheStoreCallTests : CacheStoreCallTestFixture
    {
        protected override CachingOptions Options => CachingOptions.Disabled;
        protected override Result Result { get; } = new DisabledCachingException();
        protected override Func<Times> SingleActionHits { get; } = Times.Never;

        protected override Result<VoidUnit> ResultFactory(VoidUnit args)
        {
            return new DisabledCachingException();
        }
    }
}