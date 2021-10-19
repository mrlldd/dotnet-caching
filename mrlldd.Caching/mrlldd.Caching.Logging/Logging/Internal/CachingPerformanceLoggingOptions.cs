using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Logging.Internal
{
    internal record CachingPerformanceLoggingOptions : ICachingPerformanceLoggingOptions
    {
        public CachingPerformanceLoggingOptions(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }

        public LogLevel LogLevel { get; }
    }
}