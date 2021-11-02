using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Serializers;

namespace mrlldd.Caching.Extensions
{
    /// <summary>
    ///     The class that contains extensions methods for dependency injection of System.Text.Json caching serializer.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     The method used to register System.Text.Json serializer for specific caching flag in singleton scope.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="options">The json serializer options.</param>
        /// <typeparam name="TFlag">The caching flag.</typeparam>
        /// <returns>The service collection.</returns>
        public static ISerializersCachingServiceCollection<TFlag> UseSystemTextJson<TFlag>(
            this ISerializersCachingServiceCollection<TFlag> services, JsonSerializerOptions? options = null) where TFlag : CachingFlag 
            => services.Use(new SystemTextJsonCachingSerializer(options));

        /// <summary>
        ///     The method used to register System.Text.Json serializer for specific caching flag in specified scope. 
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="optionsFactory">The json serializer options factory delegate.</param>
        /// <param name="scope">The service lifetime scope.</param>
        /// <typeparam name="TFlag">The caching flag.</typeparam>
        /// <returns>The service collection.</returns>
        public static ISerializersCachingServiceCollection<TFlag> UseSystemTextJson<TFlag>(
            this ISerializersCachingServiceCollection<TFlag> services,
            Func<IServiceProvider, JsonSerializerOptions?> optionsFactory,
            ServiceLifetime scope = ServiceLifetime.Scoped) where TFlag : CachingFlag
            => services.Use(sp => new SystemTextJsonCachingSerializer(optionsFactory(sp)), scope);
    }
}