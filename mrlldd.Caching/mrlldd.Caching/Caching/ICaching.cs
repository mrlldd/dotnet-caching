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
        /// <param name="memoryCachingCache">The memory cache.</param>
        /// <param name="distributedCachingCache">The distributed cache.</param>
        /// <param name="logger">The logger.</param>
        void Populate(IMemoryCachingStore memoryCachingCache,
            IDistributedCachingStore distributedCachingCache,
            ILogger<ICaching<T>> logger);
        
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