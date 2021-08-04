using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Stores.Internal;

namespace mrlldd.Caching.Stores
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStores(this IServiceCollection services)
            => services
                .AddScoped<IMemoryCachingStore, MemoryCachingStore>()
                .AddScoped<IDistributedCachingStore, DistributedCachingStore>()
                .AddScoped<IBubbleCachingStore, BubbleCachingStore>();
    }
}