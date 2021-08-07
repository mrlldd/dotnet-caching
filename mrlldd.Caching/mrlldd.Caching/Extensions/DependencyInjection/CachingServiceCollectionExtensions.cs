using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Extensions.DependencyInjection.Internal;
using mrlldd.Caching.Stores.Decoration;
using mrlldd.Caching.Stores.Decoration.Internal.Logging;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    /// The class that contains extensions methods for dependency injection of utilities used by that library.
    /// </summary>
    public static class CachingServiceCollectionExtensions
    {
        /// <summary>
        /// The method used for adding default logging decoration of cache actions.
        /// </summary>
        /// <param name="cachingServiceCollection">The caching service collection.</param>
        /// <param name="logLevel">The log level used on logging.</param>
        /// <returns>The caching service collection.</returns>
        public static ICachingServiceCollection AddLogging(this ICachingServiceCollection cachingServiceCollection, LogLevel logLevel = LogLevel.Debug)
        {
            cachingServiceCollection.AddScoped<ICachingStoreDecorator, LoggingCachingStoreDecorator>();
            cachingServiceCollection.AddSingleton<ICachingLoggingOptions>(new CachingLoggingOptions(logLevel));
            return cachingServiceCollection;
        }

        /// <summary>
        /// The method used for registering custom decorators of caching stores.
        /// </summary>
        /// <param name="cachingServiceCollection">The caching service collection.</param>
        /// <typeparam name="T">The type of decorator class that implements <see cref="ICachingStoreDecorator"/>.</typeparam>
        /// <returns>The caching service collection.</returns>
        public static ICachingServiceCollection Decorate<T>(this ICachingServiceCollection cachingServiceCollection) where T : class, ICachingStoreDecorator
        {
            cachingServiceCollection.AddScoped<ICachingStoreDecorator, T>();
            return cachingServiceCollection;
        }
    }
}