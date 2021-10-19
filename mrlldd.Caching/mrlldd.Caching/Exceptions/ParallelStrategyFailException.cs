using System.Collections.Generic;
using Functional.Result;

namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents a partial (or overall) fail of parallel strategy method execution.
    /// </summary>
    public class ParallelStrategyFailException : CachingException
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="fails">The fails happen during execution.</param>
        public ParallelStrategyFailException(IReadOnlyCollection<Result> fails) : base(
            "Some of the parallel strategy actions have failed, see exception details.")
        {
            Fails = fails;
        }

        /// <summary>
        ///     The fails happen during execution.
        /// </summary>
        public IReadOnlyCollection<Result> Fails { get; }
    }
}