using System;
using Functional.Result;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Caches
{
    internal sealed class CacheProvider : CachingProvider, ICacheProvider
    {
        public CacheProvider(IServiceProvider serviceProvider,
            IStoreOperationProvider storeOperationProvider)
            : base(serviceProvider, storeOperationProvider)
        {
        }

        public Result<IInternalCache<T>> GetRequired<T>()
            => InternalRequiredGet<IInternalCache<T>>();

        public Result<object> GetRequired(Type type)
            => InternalRequiredGet(type);
    }
}