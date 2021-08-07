using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Decoration.Internal.Logging.Actions;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Logging.Internal;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    public static class CachingServiceCollectionExtensions
    {
        /// <summary>
        /// The method used for adding default logging decoration of cache actions.
        /// </summary>
        /// <param name="cachingServiceCollection">The caching service collection.</param>
        /// <param name="logLevel">The log level commonly used by loggers.</param>
        /// <param name="errorsLogLevel">The log level used by loggers in case of error.</param>
        /// <returns>The caching service collection.</returns>
        public static ICachingServiceCollection AddActionsLogging(this ICachingServiceCollection cachingServiceCollection,
            LogLevel logLevel = LogLevel.Debug, LogLevel errorsLogLevel = LogLevel.Error)
        {
            cachingServiceCollection.AddScoped<ICacheStoreDecorator, ActionsLoggingCacheStoreDecorator>();
            cachingServiceCollection.AddSingleton<ICachingActionsLoggingOptions>(new CachingActionsLoggingOptions(logLevel, errorsLogLevel));
            return cachingServiceCollection;
        }
    }
}