using DryIoc;
using Microsoft.Extensions.Caching.Distributed;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Tests.Caches.TestUtilities.Extensions
{
    public static class ContainerExtensions
    {
        public static IContainer WithNoCaches<T>(this IContainer container)
        {
            container.Register<ICache<T>, BubbleCache<T>>();
            return container;
        }
        
        public static IContainer WithFakeDistributedCache(this IContainer container)
        {
            container.Register<IDistributedCache, FakeDistributedCache>();
            return container;
        }
        
        public static IContainer WithMemoryCacheOnly<T>(this IContainer container)
        {
            container.Register<ICache<T>, OnlyMemoryCache<T>>();
            return container;
        }
        
        public static IContainer WithDistributedCacheOnly<T>(this IContainer container)
        {
            container.Register<ICache<T>, OnlyDistributedCache<T>>();
            return container;
        }
    }
}