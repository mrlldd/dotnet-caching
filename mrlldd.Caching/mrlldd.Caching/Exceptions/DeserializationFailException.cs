using System;

namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents deserialization fail.
    /// </summary>
    public class DeserializationFailException : CachingException
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="value">The value.</param>
        /// <param name="valueType">The type of value.</param>
        /// <param name="exception">The inner exception.</param>
        public DeserializationFailException(string key, byte[] value, Type valueType, Exception exception) : base(
            $"Failed to deserialize entry with key '{key}' to instance of type '{valueType.FullName}'. See exception properties.", exception)
        {
            Key = key;
            Value = value;
            ValueType = valueType;
        }

        /// <summary>
        ///     The cache entry key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        ///     Value used for deserialization.
        /// </summary>
        public byte[] Value { get; }

        /// <summary>
        ///     The type of value.
        /// </summary>
        public Type ValueType { get; }
    }
}