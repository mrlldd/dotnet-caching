using System;
using System.Collections.Generic;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching
{
    internal sealed class CachingProvider
    {
        private readonly IDictionary<Type, object> scopedServicesCache = new Dictionary<Type, object>();
        private readonly IServiceProvider serviceProvider;
        private readonly IStoreOperationProvider storeOperationProvider;

        public CachingProvider(IServiceProvider serviceProvider,
            IStoreOperationProvider storeOperationProvider)
        {
            this.serviceProvider = serviceProvider;
            this.storeOperationProvider = storeOperationProvider;
        }

        private void Populate(ICaching target)
        {
            target.Populate(serviceProvider, storeOperationProvider);
        }

        public Result<object> GetRequired(Type type)
        {
            if (!typeof(ICaching).IsAssignableFrom(type))
                return new ArgumentException(
                        $"Type '{type.FullName}' is not assignable to '${typeof(ICaching).FullName}'.", nameof(type))
                    .AsFail<object>();
            return scopedServicesCache.TryGetValue(type, out var raw) && raw is ICaching
                ? raw.AsSuccess()
                : Result.Of(() =>
                {
                    var found = serviceProvider.GetRequiredService(type);
                    Populate((ICaching) found);
                    scopedServicesCache[type] = found;
                    return found;
                });
        }
    }
}