using mrlldd.Caching.Serializers;

namespace mrlldd.Caching.Stores.Internal
{
    internal record CacheStoreOperationOptions : ICacheStoreOperationOptions
    {
        public CacheStoreOperationOptions(int id, string delimiter, ICachingSerializer serializer)
        {
            OperationId = id;
            Delimiter = delimiter;
            Serializer = serializer;
        }

        public int OperationId { get; }
        public string Delimiter { get; }
        public ICachingSerializer Serializer { get; }
    }
}