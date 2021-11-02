using mrlldd.Caching.Serializers;

namespace mrlldd.Caching.Stores
{
    /// <summary>
    ///     The interface that represents service for handling store operations.
    /// </summary>
    public interface IStoreOperationOptionsProvider
    {
        /// <summary>
        ///     The method that handles uniqueness of every new operation.
        /// </summary>
        /// <param name="cacheKeyDelimiter">The cache key delimiter.</param>
        /// <param name="serializer">The caching serializer.</param>
        /// <returns>The cache store operation metadata.</returns>
        ICacheStoreOperationOptions Next(string cacheKeyDelimiter, ICachingSerializer serializer);
    }
}