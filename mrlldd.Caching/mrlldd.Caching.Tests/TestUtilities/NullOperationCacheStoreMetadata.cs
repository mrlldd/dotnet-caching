using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Tests.TestUtilities
{
    public class NullOperationCacheStoreMetadata : ICacheStoreOperationMetadata
    {
        private NullOperationCacheStoreMetadata()
        {}

        public static ICacheStoreOperationMetadata Instance { get; } = new NullOperationCacheStoreMetadata();

        public int OperationId => -1;
        public string Delimiter => "null-delimiter";
    }
}