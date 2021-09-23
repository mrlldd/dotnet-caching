using Moq;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Tests.Store.Base;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Store
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
}