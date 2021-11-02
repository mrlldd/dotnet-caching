using System.Threading;
using mrlldd.Caching.Serializers;

namespace mrlldd.Caching.Stores.Internal
{
    internal class StoreOperationOptionsProvider : IStoreOperationOptionsProvider
    {
        private int currentId = 1;

        public ICacheStoreOperationOptions Next(string cacheKeyDelimiter, ICachingSerializer serializer)
        {
            return new CacheStoreOperationOptions(Interlocked.Increment(ref currentId), cacheKeyDelimiter, serializer);
        }
    }
}