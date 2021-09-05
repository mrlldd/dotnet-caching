using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.DependencyInjection.Internal;

namespace mrlldd.Caching.Loaders
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoaders(this IServiceCollection services, Assembly assembly)
            => services
                .AddScoped<ILoaderProvider, LoaderProvider>()
                .AddScoped<ICachingLoader, CachingLoader>()
                .WithCollectedServices(assembly, typeof(CachingLoader<,>), typeof(ICachingLoader<,>));
    }
}