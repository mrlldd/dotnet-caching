namespace mrlldd.Caching.Models
{
    /// <summary>
    /// The cache config interface.
    /// </summary>
    public interface ICacheConfig
    {
        /// <summary>
        /// The cache connection string.
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// The linear retries count.
        /// </summary>
        int LinearRetries { get; }
        /// <summary>
        /// The keep alive seconds for items in distributed cache.
        /// </summary>
        int KeepAliveSeconds { get; }
    }
}