using System;

namespace mrlldd.Caching
{
    /// <summary>
    /// The class that represents a caching options used to set up the caches.
    /// </summary>
    public class CachingOptions
    {
        private CachingOptions(bool shouldCache, TimeSpan slidingExpiration)
        {
            IsCaching = shouldCache;
            SlidingExpiration = slidingExpiration;
        }

        internal bool IsCaching { get; }
        /// <summary>
        /// The cache item expiration timeout.
        /// </summary>
        public TimeSpan SlidingExpiration { get; }
        
        private static readonly TimeSpan DisabledTimeout = TimeSpan.FromMilliseconds(1);

        /// <summary>
        /// Options that represents a disabled caching.
        /// </summary>
        public static readonly CachingOptions Disabled = new(false, DisabledTimeout);
        
        /// <summary>
        /// The factory method used for creating an enabled caching.
        /// </summary>
        /// <param name="timeout">The cache item expiration timeout.</param>
        /// <returns>The caching options.</returns>
        public static CachingOptions Enabled(TimeSpan timeout) => new(true, timeout);
    }
}