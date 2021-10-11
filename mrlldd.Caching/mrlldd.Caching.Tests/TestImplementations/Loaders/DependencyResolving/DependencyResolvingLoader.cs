using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Loaders.DependencyResolving
{
    public class DependencyResolvingLoader : ILoader<DependencyResolvingUnit, string>
    {
        public Task<string> LoadAsync(DependencyResolvingUnit args, CancellationToken token = default)
            => Task.FromResult(args.ToString()!);
    }
}