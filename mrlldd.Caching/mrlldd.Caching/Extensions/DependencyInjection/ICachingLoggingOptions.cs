using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    /// The interface that represents logging options used by logging decorators.
    /// </summary>
    public interface ICachingLoggingOptions
    {
        /// <summary>
        /// The log level used by loggers.
        /// </summary>
        public LogLevel LogLevel { get; }
    }
}