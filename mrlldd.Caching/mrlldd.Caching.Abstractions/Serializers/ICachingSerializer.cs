using System.Threading;
using System.Threading.Tasks;
using Functional.Result;

namespace mrlldd.Caching.Serializers
{
    /// <summary>
    ///     The interface that represents serializer used in caching operations.
    /// </summary>
    public interface ICachingSerializer
    {
        /// <summary>
        ///     The method used to serialize generic value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The result of serialization.</returns>
        ValueTask<Result<byte[]>> SerializeAsync<T>(T? value, CancellationToken token = default);

        /// <summary>
        ///     The method used to deserialize raw byte array value to generic type instance.
        /// </summary>
        /// <param name="rawValue">The byte array value.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of generic result value.</typeparam>
        /// <returns>The result of deserialization.</returns>
        ValueTask<Result<T?>> DeserializeAsync<T>(byte[] rawValue, CancellationToken token = default);
    }
}