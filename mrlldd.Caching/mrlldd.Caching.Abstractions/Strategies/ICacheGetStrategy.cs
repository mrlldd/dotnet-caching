using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Strategies
{
    /// <summary>
    ///     The interface that represents cache entry getting operations strategy.
    /// </summary>
    public interface ICacheGetStrategy
    {
        /// <summary>
        ///     The method used to get entry of type <typeparamref name="T" /> from cache asynchronously.
        /// </summary>
        /// <param name="caches">The caches collection.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of entry.</typeparam>
        /// <returns>The <see cref="Task{T}" /> that returns result of getting entry of type <typeparamref name="T" />.</returns>
        Task<Result<T>> GetAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default);

        /// <summary>
        ///     The method used to get entry of type <typeparamref name="T" /> from cache synchronously.
        /// </summary>
        /// <param name="caches">The caches collection.</param>
        /// <typeparam name="T">The type of entry.</typeparam>
        /// <returns>The <see cref="Task{T}" /> that returns result of getting entry of type <typeparamref name="T" />.</returns>
        Result<T> Get<T>(IReadOnlyCachesCollection<T> caches);
    }
}