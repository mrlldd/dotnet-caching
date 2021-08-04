using System.Threading.Tasks;
using Functional.Object.Extensions;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.Loaders.TestUtilities
{
    public class TestClient : ITestClient
    {
        public Task<TestUnit> LoadAsync(TestArgument argument) 
            => TestUnit.Create()
                .Effect(x => x.PublicProperty = argument.Id)
                .Map(Task.FromResult);
    }
}