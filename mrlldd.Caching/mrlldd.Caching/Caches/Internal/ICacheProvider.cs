using System;
using Functional.Result;

namespace mrlldd.Caching.Caches.Internal
{
    /// <summary>
    /// The service used for providing caches.
    /// </summary>
    internal interface ICacheProvider
    {
        /// <summary>
        /// The method used to get a cache.
        /// </summary>
        /// <typeparam name="T">The type of stored object in cache.</typeparam>
        /// <returns>The result of getting the cache.</returns>
        Result<IInternalCache<T>> GetRequired<T>();

        /// <summary>
        /// The non-generic method used to get a cache.
        /// </summary>
        /// <param name="type">The interface type of cache (<see cref="IInternalCache{T}"/>)</param>
        /// <returns>The cache.</returns>
        Result<object> GetRequired(Type type);
    }
}