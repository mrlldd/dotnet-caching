namespace mrlldd.Caching.Models
{
    public interface ICacheConfig
    {
        string ConnectionString { get; }
        int LinearRetries { get; }
        int KeepAliveSeconds { get; }
    }
}