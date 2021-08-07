using mrlldd.Caching.Stores;

namespace mrlldd.Caching
{
    /// <summary>
    /// The interface that represents a base class for implementing caching utilities.
    /// </summary>
    public interface ICaching
    {
        /// <summary>
        /// A method used for populating that class with dependencies,
        /// created in order to reduce the boilerplate constructor code in every implementation.
        /// </summary>
        /// <param name="memoryCacheStore">The memory cache.</param>
        /// <param name="distributedCacheStore">The distributed cache.</param>
        /// <param name="storeOperationProvider">The store operation provider.</param>
        void Populate(IMemoryCacheStore memoryCacheStore,
            IDistributedCacheStore distributedCacheStore,
            IStoreOperationProvider storeOperationProvider);
        
        /// <summary>
        /// Indicates that caching service is using memory to cache data.
        /// </summary>
        bool IsUsingMemory { get; }
        /// <summary>
        /// Indicates that caching service is using distributed cache to cache data.
        /// </summary>
        bool IsUsingDistributed { get; }
    }
}