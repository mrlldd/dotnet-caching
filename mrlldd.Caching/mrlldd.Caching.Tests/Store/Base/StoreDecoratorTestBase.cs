using System.Threading;
using System.Threading.Tasks;
using DryIoc;
using Functional.Object.Extensions;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Internal;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Store.Base
{
    public abstract class StoreDecoratorTestBase : StoreRelatedTestBase
    {
        [Test]
        public void CallsGet() => Container
            .Effect(c =>
                CallsSpecific(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                    x => x.Get<VoidUnit>(It.Is<string>(s => s.Equals(Key)),
                        It.Is<ICacheStoreOperationMetadata>(m => m == NullMetadata.Instance)),
                    () => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore
                        .Get<VoidUnit>(Key, NullMetadata.Instance),
                    new VoidUnit().AsSuccess<VoidUnit?>()
                )
            );

        [Test]
        public Task CallsGetAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                    x => x.GetAsync<VoidUnit>(It.Is<string>(s => s.Equals(Key)),
                        It.Is<ICacheStoreOperationMetadata>(m => m == NullMetadata.Instance),
                    It.IsAny<CancellationToken>()),
                    () => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore
                        .GetAsync<VoidUnit>(Key, NullMetadata.Instance)
                        .AsTask(),
                    new ValueTask<Result<VoidUnit?>>(new VoidUnit().AsSuccess<VoidUnit?>())
                )
            );

        [Test]
        public void CallsSet() => Container
            .Effect(c =>
                CallsSpecific(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                    x => x.Set(It.Is<string>(s => s.Equals(Key)),
                        It.IsAny<VoidUnit>(), 
                        It.IsAny<CachingOptions>(),
                        It.Is<ICacheStoreOperationMetadata>(m => m == NullMetadata.Instance)),
                    () => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore
                        .Set(Key, new VoidUnit(), CachingOptions.Disabled, NullMetadata.Instance),
                    Result.Success
                )
            );

        [Test]
        public Task CallsSetAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                    x => x.SetAsync(It.Is<string>(s => s.Equals(Key)),
                        It.IsAny<VoidUnit>(),
                        It.IsAny<CachingOptions>(),
                        It.Is<ICacheStoreOperationMetadata>(m => m == NullMetadata.Instance),
                        It.IsAny<CancellationToken>()),
                    () => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore
                        .SetAsync(Key, new VoidUnit(), CachingOptions.Disabled,
                            NullMetadata.Instance)
                        .AsTask(),
                    new ValueTask<Result>(Result.Success)
                ));

        [Test]
        public void CallsRefresh() => Container
            .Effect(c =>
                CallsSpecific(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                    x => x.Refresh(It.Is<string>(s => s.Equals(Key)),
                        It.Is<ICacheStoreOperationMetadata>(m => m == NullMetadata.Instance)),
                    () => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore
                        .Refresh(Key, NullMetadata.Instance),
                    Result.Success
                ));

        [Test]
        public Task CallsRefreshAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                    x => x.RefreshAsync(It.Is<string>(s => s.Equals(Key)),
                        It.Is<ICacheStoreOperationMetadata>(m => m == NullMetadata.Instance),
                        It.IsAny<CancellationToken>()),
                    () => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore
                        .RefreshAsync(Key, NullMetadata.Instance)
                        .AsTask(),
                    new ValueTask<Result>(Result.Success)
                ));

        [Test]
        public void CallsRemove() => Container
            .Effect(c =>
                CallsSpecific(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                    x => x.Remove(It.Is<string>(s => s.Equals(Key)),
                        It.Is<ICacheStoreOperationMetadata>(m => m == NullMetadata.Instance)),
                    () => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore
                        .Remove(Key, NullMetadata.Instance),
                    Result.Success
                ));

        [Test]
        public void CallsRemoveAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                    x => x.RemoveAsync(It.Is<string>(s => s.Equals(Key)), 
                        It.Is<ICacheStoreOperationMetadata>(m => m == NullMetadata.Instance),
                        It.IsAny<CancellationToken>()),
                    () => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                        .CacheStore
                        .RemoveAsync(Key, NullMetadata.Instance)
                        .AsTask(),
                    new ValueTask<Result>(Result.Success)
                ));

        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            container.AddMock<ICacheStore<InVoid>>(MockRepository);
        }
    }
}