using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Extensions.DependencyInjection.Internal
{
    internal class CachingLoggingOptions : ICachingLoggingOptions
    {
        public LogLevel LogLevel { get; }
        
        public CachingLoggingOptions(LogLevel logLevel) 
            => LogLevel = logLevel;
    }
}