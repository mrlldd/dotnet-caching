namespace mrlldd.Caching.Stores.Decoration
{
    public interface IHasOrder
    {
        /// <summary>
        /// The order of applying among all registered decorators.
        /// </summary>
        public int Order { get; }
    }
}