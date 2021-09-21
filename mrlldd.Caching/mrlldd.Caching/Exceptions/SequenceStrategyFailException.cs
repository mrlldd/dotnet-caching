using System.Collections.Generic;
using Functional.Result;

namespace mrlldd.Caching.Exceptions
{
    public class SequenceStrategyFailException : CachingException
    {
        public SequenceStrategyFailException(IReadOnlyCollection<Result> fails) : base(
            "Some of the sequence strategy actions have failed.")
            => Fails = fails;

        public IReadOnlyCollection<Result> Fails { get; }
    }
}