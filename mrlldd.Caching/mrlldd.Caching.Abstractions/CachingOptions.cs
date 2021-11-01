using System;

namespace mrlldd.Caching
{
    /// <summary>
    ///     The class that represents a caching options used to set up the caches.
    /// </summary>
    public record CachingOptions
    {
        /// <summary>
        ///     Options that represents a disabled caching.
        /// </summary>
        public static readonly CachingOptions Disabled = new(false, null,null);

        private CachingOptions(bool shouldCache, TimeSpan? slidingExpiration, TimeSpan? absoluteExpirationRelativeToNow)
        {
            IsCaching = shouldCache;
            SlidingExpiration = slidingExpiration;
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
        }

        /// <summary>
        ///     The bool that indicates if caching is enabled.
        /// </summary>
        public bool IsCaching { get; }

        /// <summary>
        ///     The cache item sliding expiration timeout.
        /// </summary>
        public TimeSpan? SlidingExpiration { get; }
        
        
        /// <summary>
        ///     The cache item absolute expiration timeout relative to now. 
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; }

        /// <summary>
        ///     The factory method used for creating an enabled caching.
        /// </summary>
        /// <param name="slidingExpiration">The cache item sliding expiration timeout.</param>
        /// <returns>The caching options.</returns>
        public static CachingOptions Enabled(TimeSpan slidingExpiration) 
            => new(true, slidingExpiration, null);

        /// <summary>
        ///     The factory method used for creating an enabled caching.
        /// </summary>
        /// <param name="slidingExpiration">The cache item sliding expiration timeout.</param>
        /// <param name="absoluteExpirationRelativeToNow">The cache item absolute expiration timeout relative to now.</param>
        /// <returns>The caching options.</returns>
        public static CachingOptions Enabled(TimeSpan? slidingExpiration, TimeSpan absoluteExpirationRelativeToNow)
            => new(true, slidingExpiration, absoluteExpirationRelativeToNow);


        /// <summary>
        ///     The factory method used for creating an enabled caching.
        /// </summary>
        /// <param name="absoluteExpirationRelativeToNow">The cache item absolute expiration timeout relative to now.</param>
        /// <returns>The caching options.</returns>
        public static CachingOptions EnabledAbsoluteRelativeToNow(TimeSpan absoluteExpirationRelativeToNow)
            => new(true, null, absoluteExpirationRelativeToNow);
    }
}