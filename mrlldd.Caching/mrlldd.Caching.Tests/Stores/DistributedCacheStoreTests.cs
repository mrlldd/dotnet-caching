using System.Threading;
using System.Threading.Tasks;
using DryIoc;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Stores.Internal;
using mrlldd.Caching.Tests.Stores.Base;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores
{
    public class DistributedCacheStoreTests : StoreRelatedTestBase
    {
        [Test]
        public void CallsGet() => Container
            .Effect(c =>
                CallsSpecific(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.Get(It.Is<string>(s => s.Equals(Key))),
                    d => new DistributedCacheStore(d)
                        .Get<VoidUnit>(Key, DefaultMetadata)
                )
            );

        [Test]
        public Task CallsGetAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.GetAsync(It.Is<string>(s => s.Equals(Key)), It.IsAny<CancellationToken>()),
                    d => new DistributedCacheStore(d)
                        .GetAsync<VoidUnit>(Key, DefaultMetadata)
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
                        It.Is<DistributedCacheEntryOptions>(o => o.SlidingExpiration == CachingOptions.SlidingExpiration)),
                    d => new DistributedCacheStore(d)
                        .Set(Key, new VoidUnit(), CachingOptions, DefaultMetadata)
                )
            );

        [Test]
        public Task CallsSetAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.SetAsync(It.Is<string>(s => s.Equals(Key)), It.IsAny<byte[]>(), It.Is<DistributedCacheEntryOptions>(o => o.SlidingExpiration == CachingOptions.SlidingExpiration), It.IsAny<CancellationToken>()),
                    d => new DistributedCacheStore(d)
                        .SetAsync(Key, new VoidUnit(), CachingOptions,  DefaultMetadata)
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
                    d => new DistributedCacheStore(d)
                        .Refresh(Key, DefaultMetadata)
                )
            );

        [Test]
        public Task CallsRefreshAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.RefreshAsync(It.Is<string>(s => s.Equals(Key)), It.IsAny<CancellationToken>()),
                    d => new DistributedCacheStore(d)
                        .RefreshAsync(Key, DefaultMetadata)
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
                    d => new DistributedCacheStore(d)
                        .Remove(Key, DefaultMetadata)
                )
            );

        [Test]
        public void CallsRemoveAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(
                    c.GetRequiredService<Mock<IDistributedCache>>(),
                    x => x.RemoveAsync(It.Is<string>(s => s.Equals(Key)), It.IsAny<CancellationToken>()),
                    d => new DistributedCacheStore(d)
                        .RemoveAsync(Key, DefaultMetadata)
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