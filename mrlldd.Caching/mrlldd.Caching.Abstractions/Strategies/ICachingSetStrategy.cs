using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Strategies
{
    /// <summary>
    ///     The interface that represents cache entry setting operations strategy.
    /// </summary>
    public interface ICachingSetStrategy
    {
        /// <summary>
        ///     The method used to set entry of type <typeparamref name="T" /> in cache asynchronously.
        /// </summary>
        /// <param name="caches">The caches collection.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of entry.</typeparam>
        /// <returns>The <see cref="Task{T}" /> that returns the setting result of entry of type <typeparamref name="T" />.</returns>
        Task<Result> SetAsync<T>(IReadOnlyCachesCollection<T> caches, T value, CancellationToken token = default);

        /// <summary>
        ///     The method used to set entry of type <typeparamref name="T" /> in cache synchronously.
        /// </summary>
        /// <param name="caches">The caches collection.</param>
        /// <param name="value">The value to set.</param>
        /// <typeparam name="T">The type of entry.</typeparam>
        /// <returns>The <see cref="Task{T}" /> that returns the setting result of entry of type <typeparamref name="T" />.</returns>
        Result Set<T>(IReadOnlyCachesCollection<T> caches, T value);
    }
}