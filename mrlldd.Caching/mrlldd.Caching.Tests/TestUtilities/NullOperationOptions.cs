using mrlldd.Caching.Serializers;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Tests.TestUtilities
{
    public class NullOperationOptions : ICacheStoreOperationOptions
    {
        private NullOperationOptions()
        {
        }

        public static ICacheStoreOperationOptions Instance { get; } = new NullOperationOptions();

        public int OperationId => -1;
        public string Delimiter => "null-delimiter";
        public ICachingSerializer Serializer => new NewtonsoftJsonCachingSerializer();
    }
}