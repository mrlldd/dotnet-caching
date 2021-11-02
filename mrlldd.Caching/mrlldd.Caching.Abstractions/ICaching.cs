using System;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Serializers;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching
{
    /// <summary>
    ///     The interface that represents a base class for implementing caching utilities.
    /// </summary>
    public interface ICaching
    {
        /// <summary>
        ///     A method used for populating that class with dependencies,
        ///     created in order to reduce the boilerplate constructor code in every implementation.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="storeOperationOptionsProvider">The store operation provider.</param>
        /// <param name="globalDefaultSerializer">The global default caching serializer.</param>
        void Populate(IServiceProvider serviceProvider,
            IStoreOperationOptionsProvider storeOperationOptionsProvider,
            ICachingSerializer globalDefaultSerializer);
    }

    /// <summary>
    ///     The interface that represents a generic base class for implementing caching utilities.
    /// </summary>
    public interface ICaching<T, TFlag> : ICaching where TFlag : CachingFlag
    {
    }
}