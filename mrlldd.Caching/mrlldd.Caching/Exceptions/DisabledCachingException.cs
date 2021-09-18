namespace mrlldd.Caching.Exceptions
{
    public class DisabledCachingException : CachingException
    {
        public DisabledCachingException() : base("Can't perform a caching operation as it's disabled.")
        {
        }
    }
}