using System;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    ///     The interface used to access methods that customize store decorations in that library.
    /// </summary>
    public interface IDecoratorsCachingServiceCollection<TFlag> : ICachingServiceCollection where TFlag : CachingFlag
    {
        /// <summary>
        ///     The method used to add decorator service for current caching flag.
        /// </summary>
        /// <param name="lifetime">The service lifetime scope of decorator.</param>
        /// <typeparam name="T">The type of decorator.</typeparam>
        /// <returns>The decorators builder collection.</returns>
        IDecoratorsCachingServiceCollection<TFlag> Add<T>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where T : class, ICacheStoreDecorator<TFlag>;

        /// <summary>
        ///     The method used to add decorator service in scoped lifetime scope for current caching flag.
        /// </summary>
        /// <param name="implementationFactory">The method used to create decorator.</param>
        /// <typeparam name="T">The type of decorator.</typeparam>
        /// <returns>The decorators builder collection.</returns>
        IDecoratorsCachingServiceCollection<TFlag> Add<T>(Func<IServiceProvider, T> implementationFactory)
            where T : class, ICacheStoreDecorator<TFlag>;


        /// <summary>
        ///     The method used to add decorator service in singleton lifetime scope for current caching flag.
        /// </summary>
        /// <param name="instance">The decorator instance.</param>
        /// <typeparam name="T">The type of decorator.</typeparam>
        /// <returns>The decorators builder collection.</returns>
        IDecoratorsCachingServiceCollection<TFlag> Add<T>(T instance) where T : class, ICacheStoreDecorator<TFlag>;
    }
}