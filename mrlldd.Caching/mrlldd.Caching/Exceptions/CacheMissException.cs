namespace mrlldd.Caching.Exceptions
{
    public class CacheMissException : CachingException
    {
        public CacheMissException(string key) : base($"Cache miss with key '${key}'.")
        {
        }
    }
}