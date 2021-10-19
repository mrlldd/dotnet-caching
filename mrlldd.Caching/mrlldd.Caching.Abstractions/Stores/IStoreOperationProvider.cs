namespace mrlldd.Caching.Stores
{
    /// <summary>
    ///     The interface that represents service for handling store operations.
    /// </summary>
    public interface IStoreOperationProvider
    {
        /// <summary>
        ///     The method that handles uniqueness of every new operation.
        /// </summary>
        /// <param name="cacheKeyDelimiter">The cache key delimiter.</param>
        /// <returns>The cache store operation metadata.</returns>
        ICacheStoreOperationMetadata Next(string cacheKeyDelimiter);
    }
}