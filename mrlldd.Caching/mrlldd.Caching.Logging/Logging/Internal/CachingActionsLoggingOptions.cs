using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Logging.Internal
{
    internal record CachingActionsLoggingOptions : ICachingActionsLoggingOptions
    {
        public LogLevel LogLevel { get; }
        public LogLevel ErrorsLogLevel { get; }

        public CachingActionsLoggingOptions(LogLevel logLevel, LogLevel errorsLogLevel)
        {
            LogLevel = logLevel;
            ErrorsLogLevel = errorsLogLevel;
        }
    }
}