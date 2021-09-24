using FluentAssertions;
using Functional.Object.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Decoration.Internal.Logging.Actions;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores.Internal;
using mrlldd.Caching.Tests.Stores.Base;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores
{
    [TestFixture]
    public class ActionsLoggingVoidCacheStoreTests : LoggingStoreDecoratorTests
    {
        protected override Times Times => Times.Exactly(2);

        protected override void FillCachingServiceCollection(ICachingServiceCollection services)
        {
            base.FillCachingServiceCollection(services);
            services.WithActionsLogging<InVoid>(DefaultLogLevel);
        }
    }
    
    [TestFixture]
    public class ActionsLoggingDecorationTests : TestBase
    {
        protected override void FillCachingServiceCollection(ICachingServiceCollection services)
        {
            base.FillCachingServiceCollection(services);
            services.WithActionsLogging<InVoid>();
        }

        [Test]
        public void ProvidesAlreadyDecoratedStore() => Container
            .Effect(c => c.GetRequiredService<ICacheStoreProvider<InVoid>>().CacheStore
                .Should()
                .NotBeNull()
                .And.BeOfType<ActionsLoggingCacheStore<InVoid>>());
    }
}