using DryIoc;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.Caches.TestUtilities
{
    public class CacheRelatedTest : Test
    {
        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            container.Register<ICache<TestUnit>, TestCache<TestUnit>>();
            container.Register<ICacheProvider, CacheProvider>();
        }
    }
}