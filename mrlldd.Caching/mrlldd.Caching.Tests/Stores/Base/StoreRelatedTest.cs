using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using mrlldd.Caching.Serializers;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Internal;

namespace mrlldd.Caching.Tests.Stores.Base
{
    public class StoreRelatedTest : TestBase
    {
        protected string Key { get; private set; } = null!;

        protected CachingOptions CachingOptions { get; private set; } = null!;
        protected ICacheStoreOperationOptions DefaultOperationOptions { get; private set; } = null!;

        protected override void AfterContainerEnriching()
        {
            base.AfterContainerEnriching();
            Key = Faker.Random.String(32);
            CachingOptions = CachingOptions.Enabled(TimeSpan.FromMilliseconds(Faker.Random.Number(60, 6000)));
            DefaultOperationOptions =
                new CacheStoreOperationOptions(Faker.Random.Number(0, 99999), Faker.Random.String(0, 32), new NewtonsoftJsonCachingSerializer());
        }

        protected static void CallsSpecific<T>(Mock<T> mock,
            Expression<Action<T>> setup, Action<T> action)
            where T : class
        {
            mock.Setup(setup)
                .Verifiable();
            action(mock.Object);
            mock.Verify(setup, Times.Once);
        }

        protected static void CallsSpecific<T, TResult>(Mock<T> mock,
            Expression<Func<T, TResult>> setup, Action<T> action, TResult result = default!)
            where T : class
        {
            mock.Setup(setup)
                .Returns(result)
                .Verifiable();
            action(mock.Object);
            mock.Verify(setup, Times.Once);
        }

        protected static async Task CallsSpecificAsync<T, TResult>(Mock<T> mock,
            Expression<Func<T, TResult>> setup, Func<T, Task> asyncAction, TResult result = default!) where T : class
        {
            mock.Setup(setup)
                .Returns(result)
                .Verifiable();
            await asyncAction(mock.Object);
            mock.Verify(setup, Times.Once);
        }

        protected static async Task CallsSpecificAsync<T>(Mock<T> mock,
            Expression<Action<T>> setup, Func<T, Task> asyncAction)
            where T : class
        {
            mock.Setup(setup).Verifiable();
            await asyncAction(mock.Object);
            mock.Verify(setup, Times.Once);
        }
    }
}