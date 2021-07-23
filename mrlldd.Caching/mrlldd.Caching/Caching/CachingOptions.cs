using System;

namespace mrlldd.Caching.Caching
{
    public class CachingOptions
    {
        private CachingOptions(bool shouldCache, TimeSpan timeout)
        {
            IsCaching = shouldCache;
            Timeout = timeout;
        }

        internal bool IsCaching { get; }
        public TimeSpan Timeout { get; }
        
        public static readonly CachingOptions Disabled = new CachingOptions(false, TimeSpan.Zero);
        
        public static CachingOptions Enabled(TimeSpan timeout) => new CachingOptions(true, timeout);
    }
}