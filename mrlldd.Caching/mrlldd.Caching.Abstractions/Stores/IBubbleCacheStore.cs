namespace mrlldd.Caching.Stores
{
    /// <summary>
    /// The interface that represents store that does actually nothing.
    /// </summary>
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IBubbleCacheStore : IDistributedCacheStore, IMemoryCacheStore
    {
        
    }
}