using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Serializers;
using mrlldd.Caching.Serializers.Internal;

namespace mrlldd.Caching.Extensions.DependencyInjection.Internal
{
    
    internal class SerializersCachingServiceCollection : CachingServiceCollection,
        ISerializersCachingServiceCollection
    {
        public SerializersCachingServiceCollection(IServiceCollection services) : base(services)
        {
        }

        public ISerializersCachingServiceCollection Use(ICachingSerializer serializer)
        {
            var descriptor = ServiceDescriptor.Singleton(new CachingSerializerProvider(serializer));
            Services.Replace(descriptor);
            return this;
        }

        public ISerializersCachingServiceCollection Use(
            Func<IServiceProvider, ICachingSerializer> serializerFactory,
            ServiceLifetime scope = ServiceLifetime.Scoped)
        {
            var descriptor = ServiceDescriptor.Describe(typeof(CachingSerializerProvider),
                sp => new CachingSerializerProvider(serializerFactory(sp)), scope);
            Services.Replace(descriptor);
            return this;
        }

        public ISerializersCachingServiceCollection Use<T>(ServiceLifetime scope = ServiceLifetime.Scoped)
            where T : class, ICachingSerializer
        {
            Services.TryAddScoped<T>();
            var descriptor = ServiceDescriptor.Describe(typeof(CachingSerializerProvider),
                sp => new CachingSerializerProvider(sp.GetRequiredService<T>()), scope);
            Services.Replace(descriptor);
            return this;
        }
    }
    
    internal class SerializersCachingServiceCollection<TFlag> : CachingServiceCollection,
        ISerializersCachingServiceCollection<TFlag> where TFlag : CachingFlag
    {
        public SerializersCachingServiceCollection(IServiceCollection services) : base(services)
        {
        }

        public ISerializersCachingServiceCollection<TFlag> Use(ICachingSerializer serializer)
        {
            var descriptor = ServiceDescriptor.Singleton(new CachingSerializerProvider<TFlag>(serializer));
            Services.Replace(descriptor);
            return this;
        }

        public ISerializersCachingServiceCollection<TFlag> Use(
            Func<IServiceProvider, ICachingSerializer> serializerFactory,
            ServiceLifetime scope = ServiceLifetime.Scoped)
        {
            var descriptor = ServiceDescriptor.Describe(typeof(CachingSerializerProvider<TFlag>),
                sp => new CachingSerializerProvider<TFlag>(serializerFactory(sp)), scope);
            Services.Replace(descriptor);
            return this;
        }

        public ISerializersCachingServiceCollection<TFlag> Use<T>(ServiceLifetime scope = ServiceLifetime.Scoped)
            where T : class, ICachingSerializer
        {
            Services.TryAddScoped<T>();
            var descriptor = ServiceDescriptor.Describe(typeof(CachingSerializerProvider<TFlag>),
                sp => new CachingSerializerProvider<TFlag>(sp.GetRequiredService<T>()), scope);
            Services.Replace(descriptor);
            return this;
        }
    }
}