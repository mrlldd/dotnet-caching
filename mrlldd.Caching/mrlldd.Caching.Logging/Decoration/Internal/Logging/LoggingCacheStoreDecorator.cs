using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Decoration.Internal.Logging
{
    internal abstract class LoggingCacheStoreDecorator<TFlag> : ICacheStoreDecorator<TFlag> where TFlag : CachingFlag
    {
        protected string LogPrefix => $"{nameof(ICacheStore<TFlag>)}<{nameof(TFlag)}>";
        
        public abstract ICacheStore<TFlag> Decorate(ICacheStore<TFlag> cacheStore);
        public abstract int Order { get; }
    }
}