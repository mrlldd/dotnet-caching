using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    public interface ICachingLoggingOptions
    {
        public LogLevel LogLevel { get; }
    }
}