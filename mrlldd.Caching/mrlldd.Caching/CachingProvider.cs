using System;
using System.Collections.Generic;
using Functional.Object.Extensions;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching
{
    internal abstract class CachingProvider
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDictionary<Type, object> scopedServicesCache = new Dictionary<Type, object>();
        private readonly IStoreOperationProvider storeOperationProvider;

        protected CachingProvider(IServiceProvider serviceProvider,
            IStoreOperationProvider storeOperationProvider)
        {
            this.serviceProvider = serviceProvider;
            this.storeOperationProvider = storeOperationProvider;
        }

        private void Populate<T>(T target) where T : ICaching 
            => target.Populate(serviceProvider, storeOperationProvider);

        protected Result<T> InternalRequiredGet<T>() where T : ICaching
            => scopedServicesCache.TryGetValue(typeof(T), out var raw)
               && raw is T service
                ? service.AsSuccess()
                : Result.Of(() => serviceProvider
                    .GetRequiredService<T>()
                    .Effect(Populate)
                    .Effect(x => scopedServicesCache[typeof(T)] = x));

        protected Result<object> InternalRequiredGet(Type type)
            => scopedServicesCache.TryGetValue(type, out var raw) && raw is ICaching
                ? raw.AsSuccess()
                : Result.Of(() => serviceProvider.GetRequiredService(type)
                    .Effect(x => Populate((ICaching) x))
                    .Effect(x => scopedServicesCache[type] = x));
    }
}