using System;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Extensions.DependencyInjection.Internal
{
    internal class DecoratorsCachingServiceCollection<TFlag> : CachingServiceCollection,
        IDecoratorsCachingServiceCollection<TFlag> where TFlag : CachingFlag
    {
        public DecoratorsCachingServiceCollection(IServiceCollection services)
            : base(services)
        {
        }


        public IDecoratorsCachingServiceCollection<TFlag> Add<T>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, ICacheStoreDecorator<TFlag>
        {
            var descriptor = ServiceDescriptor.Describe(typeof(ICacheStoreDecorator<TFlag>), typeof(T), lifetime);
            Services.Add(descriptor);
            return this;
        }

        public IDecoratorsCachingServiceCollection<TFlag> Add<T>(Func<IServiceProvider, T> implementationFactory)
            where T : class, ICacheStoreDecorator<TFlag>
        {
            Services.AddScoped<ICacheStoreDecorator<TFlag>, T>(implementationFactory);
            return this;
        }

        public IDecoratorsCachingServiceCollection<TFlag> Add<T>(T instance)
            where T : class, ICacheStoreDecorator<TFlag>
        {
            Services.AddSingleton<ICacheStoreDecorator<TFlag>>(instance);
            return this;
        }
    }
}