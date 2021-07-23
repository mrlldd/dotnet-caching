namespace mrlldd.Caching.Models
{
    public class DefaultCacheConfig : ICacheConfig
    {
        public string ConnectionString { get; set; }
        public int LinearRetries { get; set; }
        public int KeepAliveSeconds { get; set; }
    }
}