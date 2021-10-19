namespace mrlldd.Caching.Stores
{
    /// <summary>
    ///     The interface that represents store operation method metadata.
    /// </summary>
    public interface ICacheStoreOperationMetadata
    {
        /// <summary>
        ///     The store operation id.
        /// </summary>
        int OperationId { get; }

        /// <summary>
        ///     The cache key delimiter
        /// </summary>
        public string Delimiter { get; }
    }
}