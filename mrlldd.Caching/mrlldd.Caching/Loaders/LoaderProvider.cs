using System;
using Functional.Result;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Loaders.Internal;
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

        public Result<object> GetRequired(Type type)
            => InternalRequiredGet(type);
    }
}