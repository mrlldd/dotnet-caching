using System;
using Functional.Result;

namespace mrlldd.Caching.Loaders.Internal
{
    /// <summary>
    /// The service used for providing loaders.
    /// </summary>
    internal interface ILoaderProvider
    {
        /// <summary>
        /// The non-generic method used to get a caching loader.
        /// </summary>
        /// <returns>The result of getting the caching loader.</returns>
        Result<object> GetRequired(Type type);
    }
}