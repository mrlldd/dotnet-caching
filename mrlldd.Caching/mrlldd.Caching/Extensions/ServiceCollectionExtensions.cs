using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Models;

namespace mrlldd.Caching.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services,
            Assembly assembly,
            ICacheConfig config)
            => services
                .AddCaches(assembly, config)
                .AddLoaders(assembly);
    }
}