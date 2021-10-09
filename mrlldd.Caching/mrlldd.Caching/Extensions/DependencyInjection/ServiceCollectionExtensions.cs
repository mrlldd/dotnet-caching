using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection.Internal;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Internal;

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
        /// <param name="assemblies">The assembly that contains implemented cached utilities and used in order to collect those types and add them to container.</param>
        /// <returns>The service collection.</returns>
        public static ICachingServiceCollection AddCaching(this IServiceCollection services,
            params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
            {
                throw new InvalidOperationException(
                    "Can't find any caching implementations in empty assemblies array.");
            }

            var types = assemblies
                .SelectMany(a => a.GetTypes())
                .ToArray();
            services
                .TryAddScoped<IStoreOperationProvider, StoreOperationProvider>();
            services
                .AddCaches(types)
                .AddLoaders(types)
                .AddCachingStores();
            return new CachingServiceCollection(services);
        }
    }
}