using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Internal;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;

namespace mrlldd.Caching.Tests
{
    public abstract class CachingTest : TestBase
    {
        private static readonly Expression<Func<IStoreOperationProvider, ICacheStoreOperationMetadata>>
            OperationProviderSetupExpression = x => x.Next(It.IsAny<string>());

        protected override void AfterContainerEnriching()
        {
            base.AfterContainerEnriching();
            Container.AddMock<ICacheStore<InMoq>>(MockRepository);
            Container.RegisterInstance(CachingOptions.Disabled);
        }

        protected override void FillCachingServiceCollection(ICachingServiceCollection services)
        {
            base.FillCachingServiceCollection(services);
            services.UseCachingStore<InMoq, MoqStore>();
        }

        protected void InjectInstance<T>(T instance)
        {
            Container.RegisterInstance(instance);
        }

        protected void WithExactOperationsCount(Action action, Func<Times> times)
        {
            var provider = Container.GetRequiredService<IStoreOperationProvider>();
            Container.AddMock<IStoreOperationProvider>(MockRepository);
            var mock = Container.GetRequiredService<Mock<IStoreOperationProvider>>();
            mock.Setup(OperationProviderSetupExpression)
                .Returns<string>(s => new CacheStoreOperationMetadata(Faker.Random.Number(99999), s))
                .Verifiable();
            action();
            mock.Verify(OperationProviderSetupExpression, times);
        }

        protected async Task WithExactOperationsCountAsync(Func<Task> asyncAction, Func<Times> times)
        {
            var provider = Container.GetRequiredService<IStoreOperationProvider>();
            Container.AddMock<IStoreOperationProvider>(MockRepository);
            var mock = Container.GetRequiredService<Mock<IStoreOperationProvider>>();
            mock.Setup(OperationProviderSetupExpression)
                .Returns<string>(s => new CacheStoreOperationMetadata(Faker.Random.Number(99999), s))
                .Verifiable();
            await asyncAction();
            mock.Verify(OperationProviderSetupExpression, times);
            InjectInstance(provider);
            Container.Unregister<Mock<IStoreOperationProvider>>();
        }
    }
}