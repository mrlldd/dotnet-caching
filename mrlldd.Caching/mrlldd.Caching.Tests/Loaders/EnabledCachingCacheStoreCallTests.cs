using System;
using Functional.Object.Extensions;
using Functional.Result;
using Moq;
using mrlldd.Caching.Tests.TestUtilities;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Loaders
{
    [TestFixture]
    public class EnabledCachingCacheStoreCallTests : CacheStoreCallTestFixture
    {
        protected override CachingOptions Options { get; } =
            CachingOptions.Enabled(Faker.Random.Double(0, 99999).Map(TimeSpan.FromMilliseconds));

        protected override Result Result => Result.Success;
        protected override Func<Times> SingleActionHits => Times.Once;

        protected override Result<VoidUnit> ResultFactory(VoidUnit args)
            => args;
    }
}