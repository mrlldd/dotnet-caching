using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Extensions.DependencyInjection.Internal;
using mrlldd.Caching.Stores.Decoration;
using mrlldd.Caching.Stores.Decoration.Internal.Logging;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    public static class CachingServiceCollectionExtensions
    {
        public static ICachingServiceCollection AddLogging(this ICachingServiceCollection cachingServiceCollection, LogLevel logLevel = LogLevel.Debug)
        {
            cachingServiceCollection.AddScoped<ICachingStoreDecorator, LoggingCachingStoreDecorator>();
            cachingServiceCollection.AddSingleton<ICachingLoggingOptions>(new CachingLoggingOptions(logLevel));
            return cachingServiceCollection;
        }

        public static ICachingServiceCollection Decorate<T>(this ICachingServiceCollection cachingServiceCollection) where T : class, ICachingStoreDecorator
        {
            cachingServiceCollection.AddScoped<ICachingStoreDecorator, T>();
            return cachingServiceCollection;
        }
    }
}