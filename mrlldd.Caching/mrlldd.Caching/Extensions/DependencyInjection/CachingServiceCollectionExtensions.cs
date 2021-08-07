﻿using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    /// The class that contains extensions methods for dependency injection of utilities used by that library.
    /// </summary>
    public static class CachingServiceCollectionExtensions
    {
        /// <summary>
        /// The method used for registering custom decorators of caching stores.
        /// </summary>
        /// <param name="cachingServiceCollection">The caching service collection.</param>
        /// <typeparam name="T">The type of decorator class that implements <see cref="ICacheStoreDecorator"/>.</typeparam>
        /// <returns>The caching service collection.</returns>
        public static ICachingServiceCollection Decorate<T>(this ICachingServiceCollection cachingServiceCollection) where T : class, ICacheStoreDecorator
        {
            cachingServiceCollection.AddScoped<ICacheStoreDecorator, T>();
            return cachingServiceCollection;
        }
    }
}