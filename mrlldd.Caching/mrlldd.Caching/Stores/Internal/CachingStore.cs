using Newtonsoft.Json;

namespace mrlldd.Caching.Stores.Internal
{
    internal abstract class CachingStore
    {
        protected static string Serialize<T>(T data)
            => JsonConvert.SerializeObject(data);

        protected static T? Deserialize<T>(string raw)
            => JsonConvert.DeserializeObject<T>(raw);

    }
}