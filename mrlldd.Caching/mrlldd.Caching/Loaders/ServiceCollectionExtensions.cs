using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.Internal;

namespace mrlldd.Caching.Loaders
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoaders(this IServiceCollection services, Assembly assembly)
            => services
                .AddScoped<ILoaderProvider, LoaderProvider>()
                .WithCollectedServices(assembly, typeof(CachingLoader<,>), typeof(ICachingLoader<,>));
    }
}