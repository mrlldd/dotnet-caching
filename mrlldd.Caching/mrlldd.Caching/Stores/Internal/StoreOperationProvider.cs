namespace mrlldd.Caching.Stores.Internal
{
    internal class StoreOperationProvider : IStoreOperationProvider
    {
        private readonly object forLock = new();
        private int currentId = 1;

        public ICacheStoreOperationMetadata Next()
        {
            int id;
            lock (forLock)
            {
                id = currentId++;
            }
            return new CacheStoreOperationMetadata(id);
        }
    }
}