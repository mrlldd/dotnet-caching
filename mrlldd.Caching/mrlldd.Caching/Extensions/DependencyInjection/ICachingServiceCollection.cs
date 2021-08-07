using Microsoft.Extensions.DependencyInjection;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    /// The interface-wrapper used in order to access extensions methods that customize usage of that library.
    /// </summary>
    public interface ICachingServiceCollection : IServiceCollection
    {
    }
}