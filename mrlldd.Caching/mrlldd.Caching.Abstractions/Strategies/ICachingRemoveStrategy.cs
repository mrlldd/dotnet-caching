using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Strategies
{
    /// <summary>
    ///     The interface that represents cache entry removing operations strategy.
    /// </summary>
    public interface ICachingRemoveStrategy
    {
        /// <summary>
        ///     The method used to remove entry of type <typeparamref name="T" /> in cache asynchronously.
        /// </summary>
        /// <param name="caches">The caches collection.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of entry.</typeparam>
        /// <returns>The <see cref="Task{T}" /> that returns result of entry of type <typeparamref name="T" /> removing.</returns>
        Task<Result> RemoveAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default);

        /// <summary>
        ///     The method used to remove entry of type <typeparamref name="T" /> in cache synchronously.
        /// </summary>
        /// <param name="caches">The caches collection.</param>
        /// <typeparam name="T">The type of entry.</typeparam>
        /// <returns>The <see cref="Task{T}" /> that returns result of entry of type <typeparamref name="T" /> removing.</returns>
        Result Remove<T>(IReadOnlyCachesCollection<T> caches);
    }
}