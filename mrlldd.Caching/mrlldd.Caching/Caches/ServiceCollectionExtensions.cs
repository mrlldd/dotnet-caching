using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.Internal;
using mrlldd.Caching.Models;
using StackExchange.Redis;

namespace mrlldd.Caching.Caches
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services, Assembly assembly, ICacheConfig config) 
            => services
                .AddMemoryCache()
                .AddStackExchangeRedisCache(x =>
                {
                    var options = x.ConfigurationOptions = ConfigurationOptions.Parse(config.ConnectionString);
                    options.ReconnectRetryPolicy = new LinearRetry(config.LinearRetries);
                    options.KeepAlive = config.KeepAliveSeconds;
                })
                .AddScoped<ICacheProvider, CacheProvider>()
                .WithCollectedServices(assembly, typeof(Cache<>), typeof(ICache<>));

    }
}