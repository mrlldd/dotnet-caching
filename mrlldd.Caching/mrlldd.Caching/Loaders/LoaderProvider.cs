using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Loaders
{
    internal sealed class LoaderProvider : CachingProvider, ILoaderProvider
    {
        public LoaderProvider(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        { }

        public ICachingLoader<TArgs, TResult> Get<TArgs, TResult>()
            where TResult : class
            => InternalGet<ICachingLoader<TArgs, TResult>, TResult>();
    }
}