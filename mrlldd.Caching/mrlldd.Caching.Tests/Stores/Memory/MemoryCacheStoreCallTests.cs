using System.Threading.Tasks;
using DryIoc;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Stores.Internal;
using mrlldd.Caching.Tests.Stores.Base;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores.Memory
{
    [TestFixture]
    public class MemoryCacheStoreCallTests : StoreRelatedTestBase
    {
        [Test]
        public void CallsGet() => Container
            .Effect(c =>
            {
                object obj;
                CallsSpecific(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.TryGetValue(It.Is<string>(s => s.Equals(Key)), out obj),
                    m => new MemoryCacheStore(m)
                        .Get<VoidUnit>(Key, DefaultMetadata)
                );
            });

        [Test]
        public Task CallsGetAsync() => Container
            .EffectAsync(c =>
            {
                object obj;
                return CallsSpecificAsync(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.TryGetValue(It.Is<string>(s => s.Equals(Key)), out obj),
                    m => new MemoryCacheStore(m)
                        .GetAsync<VoidUnit>(Key, DefaultMetadata).AsTask());
            });

        [Test]
        public void CallsSet() => Container
            .Effect(c =>
                CallsSpecific(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.CreateEntry(It.Is<string>(s => s.Equals(Key))),
                    m => new MemoryCacheStore(m)
                        .Set(Key, new VoidUnit(), CachingOptions, DefaultMetadata)
                ));

        [Test]
        public Task CallsSetAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.CreateEntry(It.Is<string>(s => s.Equals(Key))),
                    m => new MemoryCacheStore(m)
                        .SetAsync(Key, new VoidUnit(), CachingOptions,
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
                    m => new MemoryCacheStore(m)
                        .Refresh(Key, DefaultMetadata)
                );
            });

        [Test]
        public Task CallsRefreshAsync() => Container
            .EffectAsync(c =>
            {
                object obj;
                return CallsSpecificAsync(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.TryGetValue(It.Is<string>(s => s.Equals(Key)), out obj),
                    m => new MemoryCacheStore(m)
                        .RefreshAsync(Key, DefaultMetadata)
                        .AsTask()
                );
            });

        [Test]
        public void CallsRemove() => Container
            .Effect(c =>
                CallsSpecific(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.Remove(It.Is<string>(s => s.Equals(Key))),
                    m => new MemoryCacheStore(m)
                        .Remove(Key, DefaultMetadata)
                ));

        [Test]
        public void CallsRemoveAsync() => Container
            .EffectAsync(c =>
                CallsSpecificAsync(c.GetRequiredService<Mock<IMemoryCache>>(),
                    x => x.Remove(It.Is<string>(s => s.Equals(Key))),
                    m => new MemoryCacheStore(m)
                        .RemoveAsync(Key, DefaultMetadata)
                        .AsTask()
                ));

        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            container.AddMock<IMemoryCache>(MockRepository);
        }
    }
}