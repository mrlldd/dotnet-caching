using System;
using System.Collections.Generic;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Serializers;
using mrlldd.Caching.Serializers.Internal;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching
{
    internal sealed class CachingProvider
    {
        private readonly IDictionary<Type, object> scopedServicesCache = new Dictionary<Type, object>();
        private readonly IServiceProvider serviceProvider;
        private readonly ICachingSerializer defaultSerializer;
        private readonly IStoreOperationOptionsProvider storeOperationOptionsProvider;

        public CachingProvider(IServiceProvider serviceProvider,
            CachingSerializerProvider defaultSerializer,
            IStoreOperationOptionsProvider storeOperationOptionsProvider)
        {
            this.serviceProvider = serviceProvider;
            this.defaultSerializer = defaultSerializer.Serializer;
            this.storeOperationOptionsProvider = storeOperationOptionsProvider;
        }

        private void Populate(ICaching target) 
            => target.Populate(serviceProvider, storeOperationOptionsProvider, defaultSerializer);

        public Result<object> GetRequired(Type type)
        {
            if (!typeof(ICaching).IsAssignableFrom(type))
            {
                return new ArgumentException(
                    $"Type '{type.FullName}' is not assignable to '${typeof(ICaching).FullName}'.", nameof(type));
            }

            return scopedServicesCache.TryGetValue(type, out var raw) && raw is ICaching
                ? raw
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