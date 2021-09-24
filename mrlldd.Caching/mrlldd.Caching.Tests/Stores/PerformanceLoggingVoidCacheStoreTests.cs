using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Functional.Object.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Decoration.Internal.Logging.Performance;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores.Decoration;
using mrlldd.Caching.Stores.Internal;
using mrlldd.Caching.Tests.Stores.Base;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores
{
    [TestFixture]
    public class PerformanceLoggingVoidCacheStoreTests : LoggingStoreDecoratorTests
    {
        protected override Times Times => Times.Once();

        protected override void FillCachingServiceCollection(ICachingServiceCollection services)
        {
            base.FillCachingServiceCollection(services);
            services.WithPerformanceLogging<InVoid>(DefaultLogLevel);
        }
    }

    [TestFixture]
    public class PerformanceLoggingDecorationTests : TestBase
    {
        protected override void FillCachingServiceCollection(ICachingServiceCollection services)
        {
            base.FillCachingServiceCollection(services);
            services.WithPerformanceLogging<InVoid>();
        }

        [Test]
        public void ProvidesAlreadyDecoratedStore() => Container
            .Effect(c => c.GetRequiredService<ICacheStoreProvider<InVoid>>().CacheStore
                .Should()
                .NotBeNull()
                .And.BeOfType<PerformanceLoggingCacheStore<InVoid>>());

        [Test]
        public void HasRegisteredDecorator() => Container
            .Effect(c =>
            {
                var decorators = c.GetRequiredService<IEnumerable<ICacheStoreDecorator<InVoid>>>().ToArray();
                decorators
                    .Should()
                    .NotBeNull();
                decorators.Length
                    .Should()
                    .Be(1);
                decorators[0]
                    .Should()
                    .NotBeNull()
                    .And.BeOfType<PerformanceLoggingCacheStoreDecorator<InVoid>>();
            });
    }
}