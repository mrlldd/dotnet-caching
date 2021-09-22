using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    /// <summary>
    /// The class that contains extensions methods for dependency injection of caching stores.
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// The method for registering caching stores used by caches and loaders.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCachingStores(this IServiceCollection services)
            => services
                .UseCachingStore<InMemory, MemoryCacheStore>(ServiceLifetime.Singleton)
                .UseCachingStore<InDistributed, DistributedCacheStore>()
                .UseCachingStore<InVoid, VoidCacheStore>(ServiceLifetime.Singleton);
    }
}