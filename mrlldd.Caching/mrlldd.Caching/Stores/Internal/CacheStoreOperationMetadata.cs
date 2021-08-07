namespace mrlldd.Caching.Stores.Internal
{
    internal record CacheStoreOperationMetadata : ICacheStoreOperationMetadata
    {
        public CacheStoreOperationMetadata(int id) 
            => OperationId = id;

        public int OperationId { get; }
    }
}