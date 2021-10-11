namespace mrlldd.Caching.Stores.Internal
{
    internal record CacheStoreOperationMetadata : ICacheStoreOperationMetadata
    {
        public CacheStoreOperationMetadata(int id, string delimiter)
        {
            OperationId = id;
            Delimiter = delimiter;
        }

        public int OperationId { get; }
        public string Delimiter { get; }
    }
}