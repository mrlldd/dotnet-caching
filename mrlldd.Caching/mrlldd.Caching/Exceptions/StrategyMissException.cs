using System.Collections.Generic;
using Functional.Result;

namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents fail of strategy get methods execution.
    /// </summary>
    /// <typeparam name="T">The type of cache entry.</typeparam>
    public class StrategyMissException<T> : CachingException
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="strategyName">The name of strategy.</param>
        /// <param name="fails">The fails happen during execution.</param>
        public StrategyMissException(string strategyName, IReadOnlyCollection<Result<T>> fails) : base(
            $"Failed to get cache entry with strategy '{strategyName}', see exception details.")
        {
            StrategyName = strategyName;
            Fails = fails;
        }

        /// <summary>
        ///     The name of strategy.
        /// </summary>
        public string StrategyName { get; }

        /// <summary>
        ///     The fails happen during execution.
        /// </summary>
        public IReadOnlyCollection<Result<T>> Fails { get; }
    }
}