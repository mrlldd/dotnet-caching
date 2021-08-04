using System.Threading.Tasks;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.Loaders.TestUtilities
{
    public interface ITestClient
    {
        public Task<TestUnit> LoadAsync(TestArgument args);
    }
}