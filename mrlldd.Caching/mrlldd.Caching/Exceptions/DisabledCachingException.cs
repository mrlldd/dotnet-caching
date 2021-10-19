namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents a caching service with disabled caching.
    /// </summary>
    public class DisabledCachingException : CachingException
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        public DisabledCachingException() : base("Can't perform a caching operation as it's disabled.")
        {
        }
    }
}