using DryIoc;
using Moq;

namespace mrlldd.Caching.Tests.TestUtilities.Extensions
{
    public static class MockExtensions
    {
        public static Mock<T> AddToContainer<T>(this Mock<T> mock, IContainer container) where T : class
        {
            container.RegisterInstance(mock);
            container.RegisterInstance(mock.Object);
            return mock;
        }
    }
}