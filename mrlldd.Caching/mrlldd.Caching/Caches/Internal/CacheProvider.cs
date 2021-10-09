using System;
using Functional.Result;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Caches.Internal
{
    internal sealed class CacheProvider : CachingProvider, ICacheProvider
    {
        public CacheProvider(IServiceProvider serviceProvider,
            IStoreOperationProvider storeOperationProvider)
            : base(serviceProvider, storeOperationProvider)
        {
        }

        public Result<object> GetRequired(Type type)
            => InternalRequiredGet(type);
    }
}