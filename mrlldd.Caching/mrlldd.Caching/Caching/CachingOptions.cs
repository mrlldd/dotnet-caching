using System;

namespace mrlldd.Caching.Caching
{
    /// <summary>
    /// The class that represents a caching options used to set up the caches.
    /// </summary>
    public class CachingOptions
    {
        private CachingOptions(bool shouldCache, TimeSpan timeout)
        {
            IsCaching = shouldCache;
            Timeout = timeout;
        }

        internal bool IsCaching { get; }
        /// <summary>
        /// The cache item expiration timeout.
        /// </summary>
        public TimeSpan Timeout { get; }
        
        /// <summary>
        /// Options that represents a disabled caching.
        /// </summary>
        public static readonly CachingOptions Disabled = new(false, TimeSpan.Zero);
        
        /// <summary>
        /// The factory method used for creating an enabled caching.
        /// </summary>
        /// <param name="timeout">The cache item expiration timeout.</param>
        /// <returns>The caching options.</returns>
        public static CachingOptions Enabled(TimeSpan timeout) => new(true, timeout);
    }
}