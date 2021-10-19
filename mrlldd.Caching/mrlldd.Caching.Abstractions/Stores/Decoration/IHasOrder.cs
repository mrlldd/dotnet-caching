namespace mrlldd.Caching.Stores.Decoration
{
    /// <summary>
    ///     The interface that represents something with specified order.
    /// </summary>
    public interface IHasOrder
    {
        /// <summary>
        ///     The order of applying among all registered decorators.
        /// </summary>
        public int Order { get; }
    }
}