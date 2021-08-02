using System.Threading.Tasks;
using DryIoc;
using FluentAssertions;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using mrlldd.Caching.Caches;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Caches
{
    public class CacheTests : Test
    {
        [Test]
        public Task CachesToMemory() => Container
            .Map(async c =>
            {
                MockRepository
                    .Create<IMemoryCache>()
                    .Effect(x =>
                        x.Setup(c => c.CreateEntry(It.Is<string>(s => s == $"test:testcache:{nameof(TestUnit)}")))
                            .Returns(MockRepository.Create<ICacheEntry>().Object)
                            .Verifiable())
                    .Effect(x => Container.RegisterInstance(x))
                    .Object
                    .Effect(x => Container.RegisterInstance(x));
                
                var provider = c.Resolve<ICacheProvider>();

                var cache = provider.Get<TestUnit>();

                var unit = new TestUnit();
                await cache.SetAsync(unit);
                c.Resolve<Mock<IMemoryCache>>().Verify(x => x.CreateEntry(It.Is<string>(s => s == $"test:testcache:{nameof(TestUnit)}")), Times.Once);
            });

        [Test]
        public Task KeepsEntriesImmutable() => Container
            .Map(async c =>
            {
                var provider = c.Resolve<ICacheProvider>();
                var cache = provider.Get<TestUnit>();
                var unit = new TestUnit();
                await cache.SetAsync(unit);
                var fromCache = await cache.GetAsync();
                fromCache.Should().BeEquivalentTo(unit);
            });
        
        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            container.Register<ICache<TestUnit>, TestCache<TestUnit>>();
        }
    }
}