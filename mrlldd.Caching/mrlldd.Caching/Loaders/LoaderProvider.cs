using System;
using Functional.Result;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Loaders
{
    internal sealed class LoaderProvider : CachingProvider, ILoaderProvider
    {
        public LoaderProvider(IServiceProvider serviceProvider,
            IStoreOperationProvider storeOperationProvider)
            : base(serviceProvider,
                storeOperationProvider)
        {
        }

        public Result<ICachingLoader<TArgs, TResult>> GetRequired<TArgs, TResult>()
            where TResult : class
            => InternalRequiredGet<ICachingLoader<TArgs, TResult>>();

        public Result<object> GetRequired(Type type)
            => InternalRequiredGet(type);
    }
}