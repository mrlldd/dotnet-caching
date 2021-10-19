using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Logging
{
    /// <summary>
    ///     The interface that represents logging options used by actions logging decorators.
    /// </summary>
    public interface ICachingActionsLoggingOptions
    {
        /// <summary>
        ///     The log level used by loggers.
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        ///     The log level used by loggers in case of errors.
        /// </summary>
        public LogLevel ErrorsLogLevel { get; }
    }
}