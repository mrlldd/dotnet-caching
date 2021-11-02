using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Newtonsoft.Json;

namespace mrlldd.Caching.Serializers
{
    /// <summary>
    ///     The Newtonsoft.Json caching serializer.
    /// </summary>
    public class NewtonsoftJsonCachingSerializer : ICachingSerializer
    {
        private readonly JsonSerializer serializer;

        /// <summary>
        /// The co
        /// </summary>
        /// <param name="settings"></param>
        public NewtonsoftJsonCachingSerializer(JsonSerializerSettings? settings = null) 
            => serializer = JsonSerializer.CreateDefault(settings);
        
        /// <inheritdoc/>
        public ValueTask<Result<byte[]>> SerializeAsync<T>(T? value, CancellationToken token = default)
        {
            var result = Result.Of(() =>
            {
                using var ms = new MemoryStream();
                using var sw = new StreamWriter(ms);
                using var writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, value);
                return ms.ToArray();
            });
            return new ValueTask<Result<byte[]>>(result);
        }
        
        /// <inheritdoc/>
        public ValueTask<Result<T?>> DeserializeAsync<T>(byte[] rawValue, CancellationToken token = default)
        {
            var result = Result.Of(() =>
            {
                using var ms = new MemoryStream(rawValue);
                using var sr = new StreamReader(ms);
                using var reader = new JsonTextReader(sr);
                return serializer.Deserialize<T>(reader);
            });
            return new ValueTask<Result<T?>>(result);
        }
    }
}