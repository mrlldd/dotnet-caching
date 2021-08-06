using System;
using System.Threading;
using System.Threading.Tasks;
using DryIoc;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.Caches.TestUtilities.Extensions;
using mrlldd.Caching.Tests.Loaders.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Loaders
{
    [TestFixture]
    public class LoaderTests : LoaderRelatedTest
    {
        [Test]
        public Task LoadsIfThereIsNothingCached() => Container
            .MockStores(MockRepository)
            .Map(async c =>
            {
                var argument = TestArgument.Create();
                var provider = c.Resolve<ILoaderProvider>();
                var loader = provider.Get<TestArgument, TestUnit>();
                await loader.GetOrLoadAsync(argument);
                c.Resolve<Mock<ITestClient>>()
                    .Verify(x => x.LoadAsync(It.IsAny<TestArgument>()), Times.Once);
                var key = CacheKeyFactory(argument);
                c.Resolve<Mock<IMemoryCachingStore>>()
                    .Effect(mock => mock.Verify(x =>
                        x.SetAsync(It.Is<string>(s => s == key),
                            It.Is<TestUnit>(a => a.PublicProperty == argument.Id),
                            It.IsAny<MemoryCacheEntryOptions>(),
                            It.IsAny<CancellationToken>()), Times.Once))
                    .Effect(mock => mock.Verify(x =>
                        x.GetAsync<TestUnit>(It.Is<string>(s => s == key),
                            It.IsAny<CancellationToken>()), Times.Once));

                c.Resolve<Mock<IDistributedCachingStore>>()
                    .Effect(mock => mock.Verify(x =>
                        x.SetAsync(It.Is<string>(s => s == key),
                            It.Is<TestUnit>(a => a.PublicProperty == argument.Id),
                            It.IsAny<DistributedCacheEntryOptions>(),
                            It.IsAny<CancellationToken>()), Times.Once))
                    .Effect(mock => mock.Verify(x =>
                        x.GetAsync<TestUnit>(It.Is<string>(s => s == key),
                            It.IsAny<CancellationToken>()), Times.Once));
            });

        [Test]
        public Task NotLoadingIfThereIsMemoryCachedValue() => Container
            .MockStores(MockRepository)
            .Map(async c =>
            {
                var argument = TestArgument.Create();
                var provider = c.Resolve<ILoaderProvider>();
                var loader = provider.Get<TestArgument, TestUnit>();
                await loader.GetOrLoadAsync(argument);
                await loader.GetOrLoadAsync(argument);
                c.Resolve<Mock<ITestClient>>()
                    .Verify(x => x.LoadAsync(It.IsAny<TestArgument>()), Times.Once);
                var key = CacheKeyFactory(argument);
                c.Resolve<Mock<IMemoryCachingStore>>()
                    .Verify(
                        x => x.GetAsync<TestUnit>(It.Is<string>(s => s == key),
                            It.IsAny<CancellationToken>()), Times.Exactly(2));
                c.Resolve<Mock<IDistributedCachingStore>>()
                    .Verify(
                        x => x.GetAsync<TestUnit>(It.Is<string>(s => s == key),
                            It.IsAny<CancellationToken>()), Times.Exactly(1));
            });

        [Test]
        public Task NotLoadingIfThereIsDistributedCachedValue() => Container
            .WithFakeDistributedCache()
            .MockStores(MockRepository)
            .Map(async c =>
            {
                var argument = TestArgument.Create();
                var provider = c.Resolve<ILoaderProvider>();
                var loader = provider.Get<TestArgument, TestUnit>();
                await loader.GetOrLoadAsync(argument);
                var key = CacheKeyFactory(argument);
                await c.Resolve<IMemoryCachingStore>().RemoveAsync(key);
                await loader.GetOrLoadAsync(argument);
                c.Resolve<Mock<ITestClient>>()
                    .Verify(x => x.LoadAsync(It.IsAny<TestArgument>()), Times.Once);
                c.Resolve<Mock<IMemoryCachingStore>>()
                    .Effect(memoryStore =>
                        memoryStore.Verify(
                            x => x.GetAsync<TestUnit>(It.Is<string>(s => s == key),
                                It.IsAny<CancellationToken>()),
                            Times.Exactly(2))
                    )
                    .Effect(memoryStore =>
                        memoryStore.Verify(
                            x => x.RemoveAsync(It.Is<string>(s => s == key),
                                It.IsAny<CancellationToken>()),
                            Times.Once)
                    );
                c.Resolve<Mock<IDistributedCachingStore>>()
                    .Verify(
                        x => x.GetAsync<TestUnit>(It.Is<string>(s => s == key),
                            It.IsAny<CancellationToken>()), Times.Exactly(2));
            });

        private string CacheKeyFactory(TestArgument argument)
            => $"{TestLoader.CacheKeyPrefix}:{TestLoader.GlobalCacheKey}:{argument.Id}";

        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            var client = container.Resolve<ITestClient>();
            MockRepository
                .Create<ITestClient>()
                .Effect(m => m.Setup(x => x.LoadAsync(It.IsAny<TestArgument>()))
                    .Returns<TestArgument>(client.LoadAsync)
                    .Verifiable()
                ).AddToContainer(container);
        }
    }
}