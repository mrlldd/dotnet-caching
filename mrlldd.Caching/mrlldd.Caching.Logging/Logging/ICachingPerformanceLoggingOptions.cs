using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Logging
{
    public interface ICachingPerformanceLoggingOptions
    {
        LogLevel LogLevel { get; }
    }
}