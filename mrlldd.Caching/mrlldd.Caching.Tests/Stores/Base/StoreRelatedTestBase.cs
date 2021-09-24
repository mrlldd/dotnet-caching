using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bogus;
using Functional.Object.Extensions;
using Moq;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Internal;

namespace mrlldd.Caching.Tests.Stores.Base
{
    public class StoreRelatedTestBase : TestBase
    {
        protected string Key { get; private set; } = null!;

        protected CachingOptions CachingOptions { get; private set; } = null!;
        protected ICacheStoreOperationMetadata DefaultMetadata { get; private set; } = null!;

        protected override void AfterContainerEnriching()
        {
            base.AfterContainerEnriching();
            var faker = new Faker();
            Key = faker.Random.String(32);
            CachingOptions = CachingOptions.Enabled(TimeSpan.FromMilliseconds(faker.Random.Number(60, 6000)));
            DefaultMetadata = new CacheStoreOperationMetadata(faker.Random.Number(0, 99999), faker.Random.String(0, 32));
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