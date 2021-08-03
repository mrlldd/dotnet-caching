using DryIoc;
using Microsoft.Extensions.Caching.Distributed;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Tests.Caches.TestUtilities.Extensions
{
    public static class ContainerExtensions
    {
        public static IContainer WithFakeDistributedCache(this IContainer container)
        {
            container.Register<IDistributedCache, FakeDistributedCache>();
            return container;
        }
        
        public static IContainer WithMemoryCacheOnly(this IContainer container)
        {
            container.Register<ICache<TestUnit>, OnlyMemoryCache>();
            return container;
        }
        
        public static IContainer WithDistributedCacheOnly(this IContainer container)
        {
            container.Register<ICache<TestUnit>, OnlyDistributedCache>();
            return container;
        }
    }
}