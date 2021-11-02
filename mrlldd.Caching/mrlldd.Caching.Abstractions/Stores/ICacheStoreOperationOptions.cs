using mrlldd.Caching.Serializers;

namespace mrlldd.Caching.Stores
{
    /// <summary>
    ///     The interface that represents store operation method metadata.
    /// </summary>
    public interface ICacheStoreOperationOptions
    {
        /// <summary>
        ///     The store operation id.
        /// </summary>
        int OperationId { get; }

        /// <summary>
        ///     The cache key delimiter.
        /// </summary>
        string Delimiter { get; }
        
        /// <summary>
        ///     The caching serializer.
        /// </summary>
        ICachingSerializer Serializer { get; }
    }
}