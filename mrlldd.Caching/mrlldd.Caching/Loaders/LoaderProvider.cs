using System;
using System.Collections.Generic;
using mrlldd.Caching.Caching;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Loaders
{
    internal sealed class LoaderProvider : CachingProvider, ILoaderProvider
    {
        public LoaderProvider(IServiceProvider serviceProvider,
            IMemoryCachingStore memoryCachingStore,
            IDistributedCachingStore distributedCachingStore,
            IBubbleCachingStore bubbleCachingStore,
            IEnumerable<ICachingStoreDecorator> decorators)
            : base(serviceProvider,
                memoryCachingStore,
                distributedCachingStore,
                bubbleCachingStore,
                decorators)
        {
        }

        public ICachingLoader<TArgs, TResult> Get<TArgs, TResult>()
            where TResult : class
            => InternalGet<ICachingLoader<TArgs, TResult>>();
    }
}