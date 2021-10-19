namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents cache entry miss.
    /// </summary>
    public class CacheMissException : CachingException
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="key">The cache key</param>
        public CacheMissException(string key) : base($"Cache miss with key '${key}'.")
        {
            Key = key;
        }

        /// <summary>
        ///     The cache key.
        /// </summary>
        public string Key { get; }
    }
}