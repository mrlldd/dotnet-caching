using System.Threading;

namespace mrlldd.Caching.Stores.Internal
{
    internal class StoreOperationProvider : IStoreOperationProvider
    {
        private int currentId = 1;

        public ICacheStoreOperationMetadata Next(string cacheKeyDelimiter)
        {
            return new CacheStoreOperationMetadata(Interlocked.Increment(ref currentId), cacheKeyDelimiter);
        }
    }
}