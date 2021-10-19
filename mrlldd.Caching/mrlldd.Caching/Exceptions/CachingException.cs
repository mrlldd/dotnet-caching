using System;

namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The class that represents exception happen during using that library.
    /// </summary>
    public abstract class CachingException : Exception
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public CachingException(string message) : base(message)
        {
        }
    }
}