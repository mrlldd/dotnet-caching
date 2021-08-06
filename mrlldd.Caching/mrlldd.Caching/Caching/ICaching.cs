using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Caching
{
    /// <summary>
    /// The interface that represents a base class for implementing caching utilities.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public interface ICaching<T>
    {
        /// <summary>
        /// A method used for populating that class with dependencies,
        /// created in order to reduce the boilerplate constructor code in every implementation.
        /// </summary>
        /// <param name="memoryCachingStore">The memory cache.</param>
        /// <param name="distributedCachingStore">The distributed cache.</param>
        void Populate(IMemoryCachingStore memoryCachingStore,
            IDistributedCachingStore distributedCachingStore);
        
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