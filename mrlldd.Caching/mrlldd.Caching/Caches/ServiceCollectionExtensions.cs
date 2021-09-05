using System.Reflection;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Extensions.DependencyInjection.Internal;

namespace mrlldd.Caching.Caches
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services, Assembly assembly) 
            => services
                .AddMemoryCache()
                .Effect(x => x.TryAddSingleton<IDistributedCache, NoOpDistributedCache>())
                .AddScoped<ICacheProvider, CacheProvider>()
                .AddScoped<ICache, Cache>()
                .AddSingleton<ICacheOptions>(new NullDefaultCacheOptions())
                .WithCollectedServices(assembly, typeof(Cache<>), typeof(ICache<>));
    }
}