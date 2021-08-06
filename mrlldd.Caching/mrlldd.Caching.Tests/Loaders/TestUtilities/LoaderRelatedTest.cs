using DryIoc;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.Loaders.TestUtilities
{
    public class LoaderRelatedTest : Test
    {
        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            container.Register<ICachingLoader<TestArgument, TestUnit>, TestLoader>();
            container.Register<ILoaderProvider, LoaderProvider>();
            container.RegisterInstance<ITestClient>(new TestClient());
        }
    }
}