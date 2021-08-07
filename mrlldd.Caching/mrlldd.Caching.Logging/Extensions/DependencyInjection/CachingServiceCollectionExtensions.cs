using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Decoration.Internal.Logging.Actions;
using mrlldd.Caching.Decoration.Internal.Logging.Performance;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Logging.Internal;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    /// The class that contains extensions methods for dependency injection of caching logging utilities.
    /// </summary>
    public static class CachingServiceCollectionExtensions
    {
        /// <summary>
        /// The method used for adding default logging decoration of cache actions.
        /// </summary>
        /// <param name="cachingServiceCollection">The caching service collection.</param>
        /// <param name="logLevel">The log level commonly used by loggers.</param>
        /// <param name="errorsLogLevel">The log level used by loggers in case of error.</param>
        /// <returns>The caching service collection.</returns>
        public static ICachingServiceCollection WithActionsLogging(this ICachingServiceCollection cachingServiceCollection,
            LogLevel logLevel = LogLevel.Debug, LogLevel errorsLogLevel = LogLevel.Error)
        {
            cachingServiceCollection.AddScoped<ICacheStoreDecorator, ActionsLoggingCacheStoreDecorator>();
            cachingServiceCollection.AddSingleton<ICachingActionsLoggingOptions>(new CachingActionsLoggingOptions(logLevel, errorsLogLevel));
            return cachingServiceCollection;
        }
        
        /// <summary>
        /// The method used for adding performance logging decoration of cache actions.
        /// </summary>
        /// <param name="cachingServiceCollection">The caching service collection.</param>
        /// <param name="logLevel">The log level commonly used by loggers.</param>
        /// <returns>The caching service collection.</returns>
        public static ICachingServiceCollection WithPerformanceLogging(this ICachingServiceCollection cachingServiceCollection,
            LogLevel logLevel = LogLevel.Debug)
        {
            cachingServiceCollection.AddScoped<ICacheStoreDecorator, PerformanceLoggingCacheStoreDecorator>();
            cachingServiceCollection.AddSingleton<ICachingPerformanceLoggingOptions>(new CachingPerformanceLoggingOptions(logLevel));
            return cachingServiceCollection;
        }
    }
}