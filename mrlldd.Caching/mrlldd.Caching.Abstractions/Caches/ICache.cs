using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches
{
    public interface ICache<T>
    {
        IReadOnlyCachesCollection<T> Instances { get; }
    }


    /// <summary>
    /// The base interface for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    /// <typeparam name="TFlag"></typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface ICache<T, TFlag> : IUnknownStoreCache<T>
        where TFlag : CachingFlag
    {

    }
} 