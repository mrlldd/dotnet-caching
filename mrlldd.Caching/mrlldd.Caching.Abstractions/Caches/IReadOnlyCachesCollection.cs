using System.Collections.Generic;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    ///     The interface that represents a readonly collection of caches for entries of type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of entry.</typeparam>
    public interface IReadOnlyCachesCollection<T> : IReadOnlyCollection<IUnknownStoreCache<T>>
    {
        /// <summary>
        ///     The filter method that used to retrieve caches with specific stores.
        /// </summary>
        /// <typeparam name="TFlag">The type of caching flag.</typeparam>
        /// <returns>The <see cref="IEnumerable{T}" /> with filtered caches.</returns>
        IEnumerable<ICache<T, TFlag>> WithFlag<TFlag>() where TFlag : CachingFlag;
    }
}