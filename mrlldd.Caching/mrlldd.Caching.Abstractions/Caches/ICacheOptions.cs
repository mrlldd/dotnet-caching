namespace mrlldd.Caching.Caches
{
    /// <summary>
    /// The cache options.
    /// </summary>
    public interface ICacheOptions
    {
        /// <summary>
        /// The memory cache options.
        /// </summary>
        CachingOptions MemoryCacheOptions { get; }
        
        /// <summary>
        /// The distributed cache options.
        /// </summary>
        CachingOptions DistributedCacheOptions { get; }
    }
}