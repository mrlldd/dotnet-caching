using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Logging.Internal
{
    internal record CachingActionsLoggingOptions : ICachingActionsLoggingOptions
    {
        public CachingActionsLoggingOptions(LogLevel logLevel, LogLevel errorsLogLevel)
        {
            LogLevel = logLevel;
            ErrorsLogLevel = errorsLogLevel;
        }

        public LogLevel LogLevel { get; }
        public LogLevel ErrorsLogLevel { get; }
    }
}