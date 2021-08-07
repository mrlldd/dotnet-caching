using System.Reflection;
using Functional.Object.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection.Internal;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    /// The class that contains extensions methods for dependency injection of caching utilities.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// The method used for adding the caching utilities to service container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="assembly">The assembly that contains implemented cached utilities and used in order to collect those types and add them to container.</param>
        /// <returns>The service collection.</returns>
        public static ICachingServiceCollection AddCaching(this IServiceCollection services,
            Assembly assembly)
            => services
                .AddCaches(assembly)
                .AddLoaders(assembly)
                .AddCachingStores()
                .Map(x => new CachingServiceCollection(x));
    }
}