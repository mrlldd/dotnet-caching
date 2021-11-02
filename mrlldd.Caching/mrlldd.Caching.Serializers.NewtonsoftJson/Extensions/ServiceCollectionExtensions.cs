using System;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Serializers;
using Newtonsoft.Json;

namespace mrlldd.Caching.Extensions
{
    /// <summary>
    ///     The class that contains extensions methods for dependency injection of Newtonsoft.Json caching serializer.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     The method used to register Newtonsoft.Json serializer for specific caching flag in singleton scope.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="options">The json serializer settings.</param>
        /// <typeparam name="TFlag">The caching flag.</typeparam>
        /// <returns>The service collection.</returns>
        public static ISerializersCachingServiceCollection<TFlag> UseNewtonsoftJson<TFlag>(
            this ISerializersCachingServiceCollection<TFlag> services, JsonSerializerSettings? options = null) where TFlag : CachingFlag 
            => services.Use(new NewtonsoftJsonCachingSerializer(options));

        /// <summary>
        ///     The method used to register Newtonsoft.Json serializer for specific caching flag in specified scope. 
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="optionsFactory">The json serializer settings factory delegate.</param>
        /// <param name="scope">The service lifetime scope.</param>
        /// <typeparam name="TFlag">The caching flag.</typeparam>
        /// <returns>The service collection.</returns>
        public static ISerializersCachingServiceCollection<TFlag> UseNewtonsoftJson<TFlag>(
            this ISerializersCachingServiceCollection<TFlag> services,
            Func<IServiceProvider, JsonSerializerSettings?> optionsFactory,
            ServiceLifetime scope = ServiceLifetime.Scoped) where TFlag : CachingFlag
            => services.Use(sp => new NewtonsoftJsonCachingSerializer(optionsFactory(sp)), scope);
    }
}