using System.Collections.Generic;
using Functional.Result;

namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents partial (or overall) fail of sequence strategy method execution.
    /// </summary>
    public class SequenceStrategyFailException : CachingException
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="fails">The fails happen during execution.</param>
        public SequenceStrategyFailException(IReadOnlyCollection<Result> fails) : base(
            "Some of the sequence strategy actions have failed.")
        {
            Fails = fails;
        }

        /// <summary>
        ///     The fails happen during execution.
        /// </summary>
        public IReadOnlyCollection<Result> Fails { get; }
    }
}