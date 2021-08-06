namespace mrlldd.Caching.Stores.Decoration
{
    public interface ICachingStoreDecorator
    {
        public IMemoryCachingStore Decorate(IMemoryCachingStore memoryCachingStore);

        public IDistributedCachingStore Decorate(IDistributedCachingStore distributedCachingStore);
        
        public int Order { get; }
    }
}