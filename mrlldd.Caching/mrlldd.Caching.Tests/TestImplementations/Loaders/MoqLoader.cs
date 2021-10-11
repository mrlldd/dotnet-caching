using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Loaders
{
    public class MoqLoader : ILoader<VoidUnit, VoidUnit>
    {
        public Task<VoidUnit> LoadAsync(VoidUnit args, CancellationToken token = default)
            => Task.FromResult(args);
    }
}