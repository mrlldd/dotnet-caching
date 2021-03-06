using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Decoration.Internal.Logging.Actions;
using mrlldd.Caching.Decoration.Internal.Logging.Performance;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Logging.Internal;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    ///     The class that contains extensions methods for dependency injection of caching logging utilities.
    /// </summary>
    public static class CachingServiceCollectionExtensions
    {
        /// <summary>
        ///     The method used for adding default logging decoration of cache actions.
        /// </summary>
        /// <param name="cachingServiceCollection">The caching service collection.</param>
        /// <param name="logLevel">The log level commonly used by loggers.</param>
        /// <param name="errorsLogLevel">The log level used by loggers in case of error.</param>
        /// <returns>The caching service collection.</returns>
        public static ICachingServiceCollection WithActionsLogging<TFlag>(
            this ICachingServiceCollection cachingServiceCollection,
            LogLevel logLevel = LogLevel.Debug, LogLevel errorsLogLevel = LogLevel.Error)
            where TFlag : CachingFlag
        {
            cachingServiceCollection.AddLogging();
            cachingServiceCollection
                .Decorators<TFlag>()
                .Add<ActionsLoggingCacheStoreDecorator<TFlag>>();
            cachingServiceCollection.AddSingleton<ICachingActionsLoggingOptions>(
                new CachingActionsLoggingOptions(logLevel, errorsLogLevel));
            return cachingServiceCollection;
        }

        /// <summary>
        ///     The method used for adding performance logging decoration of cache actions.
        /// </summary>
        /// <param name="cachingServiceCollection">The caching service collection.</param>
        /// <param name="logLevel">The log level commonly used by loggers.</param>
        /// <returns>The caching service collection.</returns>
        public static ICachingServiceCollection WithPerformanceLogging<TFlag>(
            this ICachingServiceCollection cachingServiceCollection,
            LogLevel logLevel = LogLevel.Debug)
            where TFlag : CachingFlag
        {
            cachingServiceCollection.AddLogging();
            cachingServiceCollection
                .Decorators<TFlag>()
                .Add<PerformanceLoggingCacheStoreDecorator<TFlag>>();
            cachingServiceCollection.AddSingleton<ICachingPerformanceLoggingOptions>(
                new CachingPerformanceLoggingOptions(logLevel));
            return cachingServiceCollection;
        }
    }
}