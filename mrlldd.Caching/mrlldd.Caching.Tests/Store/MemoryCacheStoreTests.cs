using System.Threading.Tasks;
using DryIoc;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Memory;
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
    [TestFixture]
    public class MemoryCacheStoreTests : StoreRelatedTestBase
    {
        [Test]
        public void CallsGet() => Container
            .Effect(c =>
            {
                object obj;
                CallsSpecific(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.TryGetValue(It.Is<string>(s => s.Equals(Key)), out obj),
                    () => c.GetRequiredService<ICacheStore<InMemory>>()
                        .Get<VoidUnit>(Key, NullMetadata.Instance)
                );
            });

        [Test]
        public Task CallsGetAsync() => Container
            .EffectAsync(c =>
            {
                object obj;
                return CallsSpecificAsync(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.TryGetValue(It.Is<string>(s => s.Equals(Key)), out obj),
                    () => c.GetRequiredService<ICacheStore<InMemory>>()
                        .GetAsync<VoidUnit>(Key, NullMetadata.Instance).AsTask());
            });

        [Test]
        public void CallsSet() => Container
            .Effect(c =>
                CallsSpecific(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.CreateEntry(It.Is<string>(s => s.Equals(Key))),
                    () => c.GetRequiredService<ICacheStore<InMemory>>()
                        .Set(Key, new VoidUnit(), CachingOptions.Disabled, NullMetadata.Instance)
                ));

        [Test]
        public Task CallsSetAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.CreateEntry(It.Is<string>(s => s.Equals(Key))),
                    () => c.GetRequiredService<ICacheStore<InMemory>>()
                        .SetAsync(Key, new VoidUnit(), CachingOptions.Disabled,
                            NullMetadata.Instance)
                        .AsTask()
                ));

        [Test]
        public void CallsRefresh() => Container
            .Effect(c =>
            {
                object obj;
                CallsSpecific(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.TryGetValue(It.Is<string>(s => s.Equals(Key)), out obj),
                    () => c.GetRequiredService<ICacheStore<InMemory>>()
                        .Refresh(Key, NullMetadata.Instance)
                );
            });

        [Test]
        public Task CallsRefreshAsync() => Container
            .EffectAsync(c =>
            {
                object obj;
                return CallsSpecificAsync(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.TryGetValue(It.Is<string>(s => s.Equals(Key)), out obj),
                    () => c.GetRequiredService<ICacheStore<InMemory>>()
                        .RefreshAsync(Key, NullMetadata.Instance)
                        .AsTask()
                );
            });

        [Test]
        public void CallsRemove() => Container
            .Effect(c =>
                CallsSpecific(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.Remove(It.Is<string>(s => s.Equals(Key))),
                    () => c.GetRequiredService<ICacheStore<InMemory>>()
                        .Remove(Key, NullMetadata.Instance)
                ));

        [Test]
        public void CallsRemoveAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.Remove(It.Is<string>(s => s.Equals(Key))),
                    () => c.GetRequiredService<ICacheStore<InMemory>>()
                        .RemoveAsync(Key, NullMetadata.Instance)
                        .AsTask()
                ));

        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            container.AddMock<IMemoryCache>(MockRepository);
        }
    }
}