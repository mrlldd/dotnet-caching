namespace mrlldd.Caching.Stores
{
    public class NullCacheStoreOperationMetadata : ICacheStoreOperationMetadata
    {
        private NullCacheStoreOperationMetadata() 
            => OperationId = 0;

        public static ICacheStoreOperationMetadata Instance { get; } = new NullCacheStoreOperationMetadata();
        public int OperationId { get; }
    }
}