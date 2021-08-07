namespace mrlldd.Caching.Stores.Decoration
{
    /// <summary>
    /// The interface that represents caching store decorator.
    /// </summary>
    public interface ICacheStoreDecorator
    {
        /// <summary>
        /// The method used for decorating the memory caching store.
        /// </summary>
        /// <param name="memoryCacheStore">The memory caching store.</param>
        /// <returns>The decorated (or not) memory caching store.</returns>
        public IMemoryCacheStore Decorate(IMemoryCacheStore memoryCacheStore);

        /// <summary>
        /// The method used for decorating the distributed caching store.
        /// </summary>
        /// <param name="distributedCacheStore">The distributed caching store.</param>
        /// <returns>The decorated (or not) memory caching store.</returns>
        public IDistributedCacheStore Decorate(IDistributedCacheStore distributedCacheStore);
        
        /// <summary>
        /// The order of applying among all registered decorators.
        /// </summary>
        public int Order { get; }
    }
}