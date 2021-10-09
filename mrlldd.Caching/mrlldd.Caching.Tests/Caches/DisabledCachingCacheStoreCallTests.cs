using System;
using Functional.Result;
using Moq;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Tests.TestUtilities;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Caches
{
    [TestFixture]
    public class DisabledCachingCacheStoreCallTests : CacheStoreCallTestFixture
    {
        protected override CachingOptions CachingOptions { get; } =
            CachingOptions.Disabled;

        protected override Func<Times> Hits => Times.Never;

        protected override Result Result => new DisabledCachingException();

        protected override Result<VoidUnit> ResultFactory(VoidUnit unit)
            => new Exception();
    }
}