using System.Threading;
using System.Threading.Tasks;
using DryIoc;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.Store.Base;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Store
{
    public class DistributedCacheStoreTests : StoreRelatedTestBase
    {
        [Test]
        public void CallsGet() => Container
            .Effect(c =>
                CallsSpecific(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.Get(It.Is<string>(s => s.Equals(Key))),
                    () => c.GetRequiredService<ICacheStore<InDistributed>>()
                        .Get<VoidUnit>(Key, NullMetadata.Instance)
                )
            );

        [Test]
        public Task CallsGetAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.GetAsync(It.Is<string>(s => s.Equals(Key)), It.IsAny<CancellationToken>()),
                    () => c.GetRequiredService<ICacheStore<InDistributed>>()
                        .GetAsync<VoidUnit>(Key, NullMetadata.Instance)
                        .AsTask(),
                    Task.CompletedTask
                )
            );

        [Test]
        public void CallsSet() => Container
            .Effect(c =>
                CallsSpecific(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.Set(It.Is<string>(s => s.Equals(Key)), It.IsAny<byte[]>(),
                        It.IsAny<DistributedCacheEntryOptions>()),
                    () => c.GetRequiredService<ICacheStore<InDistributed>>()
                        .Set(Key, new VoidUnit(), CachingOptions.Disabled, NullMetadata.Instance)
                )
            );

        [Test]
        public Task CallsSetAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.SetAsync(It.Is<string>(s => s.Equals(Key)), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()),
                    () => c.GetRequiredService<ICacheStore<InDistributed>>()
                        .SetAsync(Key, new VoidUnit(), CachingOptions.Disabled,  NullMetadata.Instance)
                        .AsTask(),
                    Task.CompletedTask
                )
            );

        [Test]
        public void CallsRefresh() => Container
            .Effect(c =>
                CallsSpecific(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.Refresh(
                        It.Is<string>(s => s.Equals(Key))
                    ),
                    () => c.GetRequiredService<ICacheStore<InDistributed>>()
                        .Refresh(Key, NullMetadata.Instance)
                )
            );

        [Test]
        public Task CallsRefreshAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.RefreshAsync(It.Is<string>(s => s.Equals(Key)), It.IsAny<CancellationToken>()),
                    () => c.GetRequiredService<ICacheStore<InDistributed>>()
                        .RefreshAsync(Key, NullMetadata.Instance)
                        .AsTask(),
                    Task.CompletedTask
                )
            );

        [Test]
        public void CallsRemove() => Container
            .Effect(c =>
                CallsSpecific(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.Remove(It.Is<string>(s => s.Equals(Key))),
                    () => c.GetRequiredService<ICacheStore<InDistributed>>()
                        .Remove(Key, NullMetadata.Instance)
                )
            );

        [Test]
        public void CallsRemoveAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.RemoveAsync(It.Is<string>(s => s.Equals(Key)), It.IsAny<CancellationToken>()),
                    () => c.GetRequiredService<ICacheStore<InDistributed>>()
                        .RemoveAsync(Key, NullMetadata.Instance)
                        .AsTask(),
                    Task.CompletedTask
                )
            );

        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            container.AddMock<IDistributedCache>(MockRepository);
        }
    }
}