using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DryIoc;
using FluentAssertions;
using Functional.Object.Extensions;
using Functional.Result;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;
using mrlldd.Caching.Stores.Internal;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores
{
    [TestFixture]
    public class StoreDecorationTests : TestBase
    {
        private readonly Expression<Func<ICacheStoreDecorator<InVoid>, ICacheStore<InVoid>>> decorateExpression = x =>
            x.Decorate(It.IsAny<ICacheStore<InVoid>>());

        protected override void FillCachingServiceCollection(ICachingServiceCollection services)
        {
            base.FillCachingServiceCollection(services);

            var mock = MockRepository.Create<ICacheStoreDecorator<InVoid>>();
            Container.RegisterInstance(mock);
            services
                .Decorators<InVoid>()
                .Add(mock.Object);

            mock.Setup(decorateExpression)
                .Returns<ICacheStore<InVoid>>(s => new StoreWrapper(s))
                .Verifiable();
        }

        [Test]
        public void CallsDecorator()
        {
            Container
                .Effect(c =>
                {
                    var decoratedStore = c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore;
                    var mock = c.GetRequiredService<Mock<ICacheStoreDecorator<InVoid>>>();
                    mock.Verify(decorateExpression, Times.Once);

                    decoratedStore
                        .Should()
                        .NotBeNull()
                        .And.BeOfType<StoreWrapper>();
                    var wrapper = (StoreWrapper) decoratedStore;
                    wrapper.Store
                        .Should()
                        .NotBeNull()
                        .And.BeOfType<VoidCacheStore>();
                });
        }

        [Test]
        public void CallsSomeDecorators()
        {
            Container
                .Effect(c =>
                {
                    var decoratedStore = c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore;
                    var mock = c.GetRequiredService<Mock<ICacheStoreDecorator<InVoid>>>();
                    mock.Verify(decorateExpression, Times.Once);

                    decoratedStore
                        .Should()
                        .NotBeNull()
                        .And.BeOfType<StoreWrapper>();
                    var wrapper = (StoreWrapper) decoratedStore;
                    wrapper.Store
                        .Should()
                        .NotBeNull()
                        .And.BeOfType<VoidCacheStore>();
                });
        }

        private class StoreWrapper : ICacheStore<InVoid>
        {
            public StoreWrapper(ICacheStore<InVoid> store)
            {
                Store = store;
            }

            public ICacheStore<InVoid> Store { get; }

            public Result<T?> Get<T>(string key, ICacheStoreOperationOptions operationOptions)
            {
                throw new NotImplementedException();
            }

            public ValueTask<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationOptions operationOptions,
                CancellationToken token = default)
            {
                throw new NotImplementedException();
            }

            public Result Set<T>(string key, T? value, CachingOptions options, ICacheStoreOperationOptions operationOptions)
            {
                throw new NotImplementedException();
            }

            public ValueTask<Result> SetAsync<T>(string key, T? value, CachingOptions options,
                ICacheStoreOperationOptions operationOptions,
                CancellationToken token = default)
            {
                throw new NotImplementedException();
            }

            public Result Refresh(string key, ICacheStoreOperationOptions operationOptions)
            {
                throw new NotImplementedException();
            }

            public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationOptions operationOptions,
                CancellationToken token = default)
            {
                throw new NotImplementedException();
            }

            public Result Remove(string key, ICacheStoreOperationOptions operationOptions)
            {
                throw new NotImplementedException();
            }

            public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationOptions operationOptions,
                CancellationToken token = default)
            {
                throw new NotImplementedException();
            }
        }
    }
}