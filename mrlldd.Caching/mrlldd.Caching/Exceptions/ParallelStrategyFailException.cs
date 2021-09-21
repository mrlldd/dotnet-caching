using System.Collections.Generic;
using Functional.Result;

namespace mrlldd.Caching.Exceptions
{
    public class ParallelStrategyFailException : CachingException
    {
        public ParallelStrategyFailException(IReadOnlyCollection<Result> fails) : base("Some of the parallel strategy actions have failed.") 
            => Fails = fails;

        public IReadOnlyCollection<Result> Fails { get; }
    }
}