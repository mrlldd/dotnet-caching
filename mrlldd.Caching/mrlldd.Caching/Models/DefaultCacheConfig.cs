namespace mrlldd.Caching.Models
{
    /// <summary>
    /// The default cache config implementation.
    /// </summary>
    public class DefaultCacheConfig : ICacheConfig
    {
        /// <inheritdoc />
        public string ConnectionString { get; set; }

        /// <inheritdoc />
        public int LinearRetries { get; set; }

        /// <inheritdoc />
        public int KeepAliveSeconds { get; set; }
    }
}