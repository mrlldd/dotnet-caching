using Moq;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Tests.Store.Base;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Store
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
}