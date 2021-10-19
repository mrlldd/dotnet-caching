using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Extensions.DependencyInjection
{
    /// <summary>
    ///     The interface-wrapper used in order to access extensions methods that customize usage of that library.
    /// </summary>
    public interface ICachingServiceCollection : IServiceCollection
    {
        /// <summary>
        ///     The method used to get the decorators collection for specific store with flag of type <typeparamref name="TFlag" />
        ///     .
        /// </summary>
        /// <typeparam name="TFlag">The type of caching flag.</typeparam>
        /// <returns>The decorators collection.</returns>
        IDecoratorsCachingServiceCollection<TFlag> Decorators<TFlag>() where TFlag : CachingFlag;
    }
}