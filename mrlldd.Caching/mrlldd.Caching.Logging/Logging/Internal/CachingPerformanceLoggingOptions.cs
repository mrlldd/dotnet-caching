using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Logging.Internal
{
    internal record CachingPerformanceLoggingOptions : ICachingPerformanceLoggingOptions
    {
        public LogLevel LogLevel { get; }

        public CachingPerformanceLoggingOptions(LogLevel logLevel) 
            => LogLevel = logLevel;
    }
}