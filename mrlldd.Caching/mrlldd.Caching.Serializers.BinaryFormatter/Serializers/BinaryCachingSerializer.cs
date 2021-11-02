using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Functional.Result;

namespace mrlldd.Caching.Serializers
{
    /// <summary>
    ///     The caching serializer that uses binary formatter.
    /// </summary>
    public class BinaryCachingSerializer : ICachingSerializer
    {
        private readonly BinaryFormatter formatter;

        /// <summary>
        /// The constructor.
        /// </summary>
        public BinaryCachingSerializer() 
            => formatter = new BinaryFormatter();

        /// <inheritdoc />
        public Result<byte[]> Serialize<T>(T? value)
        {
            if (value == null)
            {
                return Array.Empty<byte>();
            }

            return Result.Of(() =>
            {
                using var stream = new MemoryStream();
                formatter.Serialize(stream, value);
                return stream.ToArray();
            });
        }

        /// <inheritdoc />
        public Result<T?> Deserialize<T>(byte[] rawValue)
        {
            if (rawValue.Length == 0)
            {
                return default(T);
            }

            return Result.Of(() =>
            {
                using var stream = new MemoryStream(rawValue);
                return (T?) formatter.Deserialize(stream);
            });
        }
    }
}