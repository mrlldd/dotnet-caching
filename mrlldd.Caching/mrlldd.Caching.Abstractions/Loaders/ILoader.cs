using System.Threading;
using System.Threading.Tasks;

namespace mrlldd.Caching.Loaders
{
    /// <summary>
    ///     The interface that represents loaders used in caching loaders (see
    ///     <see cref="ICachingLoader{TArgs,TResult,TFlag}" /> loaders.
    /// </summary>
    /// <typeparam name="TArgs">Loading argument type.</typeparam>
    /// <typeparam name="TResult">Loading result type.</typeparam>
    public interface ILoader<TArgs, TResult>
    {
        /// <summary>
        ///     The method used to load <typeparamref name="TResult" /> with argument of type <typeparamref name="TArgs" />
        /// </summary>
        /// <param name="args">The arguments for loading.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The result of loading.</returns>
        Task<TResult> LoadAsync(TArgs args, CancellationToken token = default);
    }
}