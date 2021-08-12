namespace mrlldd.Caching.Caches
{
    public interface ICacheOptions
    {
        CachingOptions MemoryCacheOptions { get; }
        CachingOptions DistributedCacheOptions { get; }
    }
}