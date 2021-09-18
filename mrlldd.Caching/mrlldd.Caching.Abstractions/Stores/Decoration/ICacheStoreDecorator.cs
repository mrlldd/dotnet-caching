using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Decoration
{
    /// <summary>
    /// The interface that represents caching store decorator.
    /// </summary>
    public interface ICacheStoreDecorator<TStoreFlag> 
        where TStoreFlag : CachingFlag
    {
        /// <summary>
        /// The method used for decorating the memory caching store.
        /// </summary>
        /// <param name="cacheStore">The caching store.</param>
        /// <returns>The decorated (or not) caching store.</returns>
        public ICacheStore<TStoreFlag> Decorate(ICacheStore<TStoreFlag> cacheStore);

        /// <summary>
        /// The order of applying among all registered decorators.
        /// </summary>
        public int Order { get; }
    }
}