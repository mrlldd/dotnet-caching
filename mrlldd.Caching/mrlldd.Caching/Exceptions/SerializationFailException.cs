using System;

namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents serialization fail.
    /// </summary>
    public class SerializationFailException : CachingException
    {
        /// <summary>
        ///     The cache entry key.
        /// </summary>
        public string Key { get; }
        
        /// <summary>
        ///     The cache entry value.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        ///     The type of value.
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="value">The cache entry value.</param>
        /// <param name="valueType">The type of cache entry value.</param>
        /// <param name="exception">The inner exception.</param>
        public SerializationFailException(string key, object? value, Type valueType, Exception exception) : base($"Failed to serialize entry with key '{key}' with value of type '{valueType.FullName}'. See exception properties.", exception)
        {
            Key = key;
            Value = value;
            ValueType = valueType;
        }
    }
}