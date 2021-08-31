using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace mrlldd.Caching.Stores.Internal
{
    internal class CachingStore
    {
        protected static string Serialize<T>(T data)
            => JsonConvert.SerializeObject(data);

        protected static T? Deserialize<T>(string raw)
            => JsonConvert.DeserializeObject<T>(raw);
    }
}