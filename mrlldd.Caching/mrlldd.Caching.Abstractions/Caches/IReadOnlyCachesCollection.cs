using System.Collections.Generic;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches
{
    public interface IReadOnlyCachesCollection<T> : IReadOnlyCollection<IUnknownStoreCache<T>>
    {
        IEnumerable<ICache<T, TFlag>> WithFlag<TFlag>() where TFlag : CachingFlag;
    }
}