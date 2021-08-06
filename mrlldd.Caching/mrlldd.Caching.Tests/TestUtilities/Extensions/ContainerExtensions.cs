using DryIoc;
using Microsoft.Extensions.Caching.Distributed;

namespace mrlldd.Caching.Tests.TestUtilities.Extensions
{
    public static class ContainerExtensions
    {
        public static IContainer WithFakeDistributedCache(this IContainer container)
        {
            container.Register<IDistributedCache, FakeDistributedCache>();
            return container;
        }

    }
}