namespace mrlldd.Caching.Exceptions
{
    public class DeserializationFailException : CachingException
    {
        public string Value { get; }

        public DeserializationFailException(string key, string value) : base($"Failed to deserialize entry with key '{key}'. See exception properties.") => Value = value;
    }
}