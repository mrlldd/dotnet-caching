using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;

namespace mrlldd.Caching.Serializers
{
    /// <summary>
    ///     The System.Text.Json caching serializer.
    /// </summary>
    public class SystemTextJsonCachingSerializer : ICachingSerializer
    {
        private readonly JsonSerializerOptions? options;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="options">The json serializer options.</param>
        public SystemTextJsonCachingSerializer(JsonSerializerOptions? options = null) => this.options = options;

        /// <inheritdoc />
        public async ValueTask<Result<byte[]>> SerializeAsync<T>(T? value, CancellationToken token = default)
        {
            var result = await Result.Of(async () =>
            {
                using var ms = new MemoryStream();
                await JsonSerializer.SerializeAsync(ms, value, options, token);
                return ms.ToArray();
            });
            return result;
        }

        /// <inheritdoc />
        public async ValueTask<Result<T?>> DeserializeAsync<T>(byte[] rawValue, CancellationToken token = default)
        {
            var result = await Result.Of(async () =>
            {
                using var ms = new MemoryStream(rawValue);
                return await JsonSerializer.DeserializeAsync<T>(ms, options, token);
            });
            return result;
        }
    }
}