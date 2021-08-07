using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Stores.Internal;

namespace mrlldd.Caching.Stores
{
    /// <summary>
    /// The class that contains extensions methods for dependency injection of caching stores.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// The method for registering caching stores used by caches and loaders.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCachingStores(this IServiceCollection services)
            => services
                .AddScoped<IMemoryCacheStore, MemoryCacheStore>()
                .AddScoped<IDistributedCacheStore, DistributedCacheStore>()
                .AddScoped<IBubbleCacheStore, BubbleCacheStore>();
    }
}