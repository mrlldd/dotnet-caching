using System;
using System.Linq.Expressions;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Internal;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests
{
    public abstract class CachingTest : TestBase
    {
        protected static readonly Expression<Func<IStoreOperationProvider, ICacheStoreOperationMetadata>> OperationProviderSetupExpression =
            x => x.Next(It.IsAny<string>());

        protected override void AfterContainerEnriching()
        {
            base.AfterContainerEnriching();
            Container.AddMock<IStoreOperationProvider>(MockRepository);
            Container.AddMock<ICacheStore<InMoq>>(MockRepository);
            var mock = Container.GetRequiredService<Mock<IStoreOperationProvider>>();
            mock.Setup(OperationProviderSetupExpression)
                .Returns<string>(s => new CacheStoreOperationMetadata(Faker.Random.Number(99999), s))
                .Verifiable();
        }
        
        

        protected override void FillCachingServiceCollection(ICachingServiceCollection services)
        {
            base.FillCachingServiceCollection(services);
            services.UseCachingStore<InMoq, MoqStore>();
        }

        protected void InjectInstance<T>(T instance) 
            => Container.RegisterInstance(instance);
    }
}