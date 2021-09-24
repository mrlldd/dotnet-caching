using System;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    public interface IDecoratorsCachingServiceCollection<TFlag> : ICachingServiceCollection where TFlag : CachingFlag
    {
        IDecoratorsCachingServiceCollection<TFlag> Add<T>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, ICacheStoreDecorator<TFlag>;

        IDecoratorsCachingServiceCollection<TFlag> Add<T>(Func<IServiceProvider, T> implementationFactory)
            where T : class, ICacheStoreDecorator<TFlag>;

        IDecoratorsCachingServiceCollection<TFlag> Add<T>(T instance) where T : class, ICacheStoreDecorator<TFlag>;
    }
}