using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Tests.TestUtilities
{
    public class NullCacheStoreOperationMetadata : ICacheStoreOperationMetadata
    {
        private NullCacheStoreOperationMetadata() 
            => OperationId = 0;

        public static ICacheStoreOperationMetadata Instance { get; } = new NullCacheStoreOperationMetadata();
        public int OperationId { get; }
    }
}