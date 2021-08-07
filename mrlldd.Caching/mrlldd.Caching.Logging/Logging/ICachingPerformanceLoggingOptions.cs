using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Logging
{
    /// <summary>
    /// The interface that represents logging options used by performance logging decorators.
    /// </summary>
    public interface ICachingPerformanceLoggingOptions
    {
        /// <summary>
        /// The log level used by loggers.
        /// </summary>
        LogLevel LogLevel { get; }
    }
}