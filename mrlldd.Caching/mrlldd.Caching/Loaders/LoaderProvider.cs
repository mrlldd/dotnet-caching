using System;
using System.Collections.Generic;
using Functional.Result;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Loaders
{
    internal sealed class LoaderProvider : CachingProvider, ILoaderProvider
    {
        public LoaderProvider(IServiceProvider serviceProvider,
            IMemoryCacheStore memoryCacheStore,
            IDistributedCacheStore distributedCacheStore,
            IBubbleCacheStore bubbleCacheStore,
            IStoreOperationProvider storeOperationProvider,
            IEnumerable<ICacheStoreDecorator> decorators)
            : base(serviceProvider,
                memoryCacheStore,
                distributedCacheStore,
                bubbleCacheStore,
                storeOperationProvider,
                decorators)
        {
        }

        public Result<ICachingLoader<TArgs, TResult>> GetRequired<TArgs, TResult>()
            where TResult : class
            => InternalRequiredGet<ICachingLoader<TArgs, TResult>>();

        public Result<object> GetRequired(Type type)
            => InternalRequiredGet(type);
    }
}