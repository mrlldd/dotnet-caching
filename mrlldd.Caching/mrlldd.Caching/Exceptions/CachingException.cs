using System;

namespace mrlldd.Caching.Exceptions
{
    public abstract class CachingException : Exception
    {
        public CachingException(string message) : base(message)
        {
        }
    }
}