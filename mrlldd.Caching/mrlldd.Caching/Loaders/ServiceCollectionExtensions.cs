using System.Linq;
using System.Reflection;
using Functional.Object.Extensions;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection.Internal;
using mrlldd.Caching.Extensions.Internal;
using mrlldd.Caching.Internal;

namespace mrlldd.Caching.Loaders
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoaders(this IServiceCollection services, Assembly assembly)
            => services
                .AddScoped<ILoaderProvider, LoaderProvider>()
                .AddScoped<ICachingLoader, CachingLoader>()
                .Map(sc =>
                {
                    var loaderTypes = assembly
                        .CollectServices(typeof(ICachingLoader<,>), typeof(CachingLoader<,>),
                            typeof(IInternalLoaderService<,>));
                    return loaderTypes
                        .Aggregate(sc, (prev, next) =>
                        {
                            var (implementation, service, markerInterface) = next;
                            return prev
                                .AddScoped(markerInterface, implementation)
                                .AddScoped(service,
                                    sp =>
                                    {
                                        var result = sp.GetRequiredService<ILoaderProvider>()
                                            .GetRequired(markerInterface);
                                        return result.Successful
                                            ? result.UnwrapAsSuccess()
                                            : throw result;
                                    });
                        });
                });
    }
}