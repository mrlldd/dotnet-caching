namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The interface that represents deserialization fail.
    /// </summary>
    public class DeserializationFailException : CachingException
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="value">The value.</param>
        public DeserializationFailException(string key, string value) : base(
            $"Failed to deserialize entry with key '{key}'. See exception properties.")
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        ///     The cache entry key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        ///     Value used for deserialization.
        /// </summary>
        public string Value { get; }
    }
}