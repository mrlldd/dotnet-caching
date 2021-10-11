using DryIoc;
using Moq;

namespace mrlldd.Caching.Tests.TestUtilities.Extensions
{
    public static class ContainerExtensions
    {
        public static IContainer AddMock<T>(this IContainer container, MockRepository mockRepository) where T : class
        {
            var mock = mockRepository.Create<T>();
            container.RegisterInstance(mock);
            container.RegisterInstance(mock.Object);
            return container;
        }
    }
}