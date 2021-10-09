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
        /// The non-generic method used to get a cache.
        /// </summary>
        /// <param name="type">The interface type of cache (<see cref="IInternalCache{T}"/>)</param>
        /// <returns>The cache.</returns>
        Result<object> GetRequired(Type type);
    }
}