using System;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Serializers;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    ///     The interface used to access methods that customize store serializers in that library.
    /// </summary>
    public interface ISerializersCachingServiceCollection<TFlag> : ICachingServiceCollection where TFlag : CachingFlag
    {
        /// <summary>
        ///     The method used to register serializer for specific caching flag in singleton scope.
        /// </summary>
        /// <param name="serializer">The caching serializer instance.</param>
        /// <returns>The service collection.</returns>
        ISerializersCachingServiceCollection<TFlag> Use(ICachingSerializer serializer);

        /// <summary>
        ///     The method used to register serializer for specific caching flag in specified scope.
        /// </summary>
        /// <param name="serializerFactory">The caching serializer factory delegate.</param>
        /// <param name="scope">The service lifetime scope.</param>
        /// <returns>The service collection.</returns>
        ISerializersCachingServiceCollection<TFlag> Use(Func<IServiceProvider, ICachingSerializer> serializerFactory, ServiceLifetime scope = ServiceLifetime.Scoped);

        /// <summary>
        ///     The method used to register serializer for specific caching flog in specified scope.
        /// </summary>
        /// <param name="scope">The service lifetime scope.</param>
        /// <typeparam name="T">The type of caching serializer service.</typeparam>
        /// <returns>The service collection.</returns>
        ISerializersCachingServiceCollection<TFlag> Use<T>(ServiceLifetime scope = ServiceLifetime.Scoped) where T : class, ICachingSerializer;
    }
}