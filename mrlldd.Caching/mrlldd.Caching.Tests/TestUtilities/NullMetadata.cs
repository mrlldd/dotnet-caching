using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Tests.TestUtilities
{
    public class NullMetadata : ICacheStoreOperationMetadata
    {
        private NullMetadata()
        {}

        public static ICacheStoreOperationMetadata Instance { get; } = new NullMetadata();

        public int OperationId => -1;
        public string Delimiter => "null-delimiter";
    }
}