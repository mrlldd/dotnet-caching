namespace mrlldd.Caching.Stores.Decoration
{
    /// <summary>
    /// The interface that represents caching store decorator.
    /// </summary>
    public interface ICachingStoreDecorator
    {
        /// <summary>
        /// The method used for decorating the memory caching store.
        /// </summary>
        /// <param name="memoryCachingStore">The memory caching store.</param>
        /// <returns>The decorated (or not) memory caching store.</returns>
        public IMemoryCachingStore Decorate(IMemoryCachingStore memoryCachingStore);

        /// <summary>
        /// The method used for decorating the distributed caching store.
        /// </summary>
        /// <param name="distributedCachingStore">The distributed caching store.</param>
        /// <returns>The decorated (or not) memory caching store.</returns>
        public IDistributedCachingStore Decorate(IDistributedCachingStore distributedCachingStore);
        
        /// <summary>
        /// The order of applying among all registered decorators.
        /// </summary>
        public int Order { get; }
    }
}