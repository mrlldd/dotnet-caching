using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents missing store with specific flag of type <typeparamref name="TFlag" />
    /// </summary>
    /// <typeparam name="TFlag">The type of caching flag.</typeparam>
    public class StoreNotFoundException<TFlag> : CachingException where TFlag : CachingFlag
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        public StoreNotFoundException() : base(
            $"Store for flag '{typeof(TFlag).FullName}' has not been found. Seems like it has not been registered.")
        {
        }
    }
}