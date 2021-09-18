using System;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching
{
    /// <summary>
    /// The interface that represents a base class for implementing caching utilities.
    /// </summary>
    public interface ICaching
    {
        /// <summary>
        /// A method used for populating that class with dependencies,
        /// created in order to reduce the boilerplate constructor code in every implementation.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="storeOperationProvider">The store operation provider.</param>
        void Populate(IServiceProvider serviceProvider,
            IStoreOperationProvider storeOperationProvider);
    }
}