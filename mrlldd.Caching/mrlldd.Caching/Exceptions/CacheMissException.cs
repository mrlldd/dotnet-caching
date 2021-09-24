namespace mrlldd.Caching.Exceptions
{
    public class CacheMissException : CachingException
    {
        public string Key { get; }
        public CacheMissException(string key) : base($"Cache miss with key '${key}'.")
        {
            Key = key;
        }
    }
}