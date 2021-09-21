using System.Collections.Generic;
using Functional.Result;

namespace mrlldd.Caching.Exceptions
{
    public class StrategyMissException<T> : CachingException
    {
        public IReadOnlyCollection<Result<T>> Fails { get; }

        public StrategyMissException(string strategyName, IReadOnlyCollection<Result<T>> fails) : base($"Failed to get cache entry with strategy '{strategyName}'.") 
            => Fails = fails;
    }
}