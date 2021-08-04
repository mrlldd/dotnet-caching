using System.Text.Json;
using Functional.Result;

namespace mrlldd.Caching.Stores.Internal
{
    internal class CachingStore
    {
        protected static byte[] Serialize<T>(T data)
            => JsonSerializer.SerializeToUtf8Bytes(data);

        protected static T Deserialize<T>(byte[] raw)
            => JsonSerializer.Deserialize<T>(raw);
    }
}