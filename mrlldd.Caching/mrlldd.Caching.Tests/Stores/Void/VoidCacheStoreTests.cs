using System.Threading.Tasks;
using FluentAssertions;
using Functional.Object.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.Stores.Base;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores.Void
{
    [TestFixture]
    public class VoidCacheStoreTests : StoreRelatedTest
    {
        [Test]
        public void FailOnAnyGet()
        {
            Container
                .Effect(c => c.GetRequiredService<ICacheStore<InVoid>>().Get<VoidUnit>(Key, DefaultOperationOptions)
                    .Should()
                    .BeFailResult<VoidUnit>()
                    .WithException<VoidUnit, CacheMissException>()
                    .Which.Key.Should().BeEquivalentTo(Key));
        }

        [Test]
        public Task FailOnAnyGetAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var result = await c.GetRequiredService<ICacheStore<InVoid>>()
                        .GetAsync<VoidUnit>(Key, DefaultOperationOptions);
                    result
                        .Should()
                        .BeFailResult<VoidUnit>()
                        .WithException<VoidUnit, CacheMissException>()
                        .Which.Key.Should().BeEquivalentTo(Key);
                });
        }

        [Test]
        public void SuccessOnAnySet()
        {
            Container
                .Effect(c => c.GetRequiredService<ICacheStore<InVoid>>()
                    .Set(Key, new VoidUnit(), CachingOptions, DefaultOperationOptions)
                    .Should()
                    .BeSuccessfulResult());
        }

        [Test]
        public Task SuccessOnAnySetAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var result = await c.GetRequiredService<ICacheStore<InVoid>>()
                        .SetAsync(Key, new VoidUnit(), CachingOptions, DefaultOperationOptions);
                    result
                        .Should()
                        .BeSuccessfulResult();
                });
        }

        [Test]
        public void SuccessOnAnyRefresh()
        {
            Container
                .Effect(c => c.GetRequiredService<ICacheStore<InVoid>>()
                    .Refresh(Key, DefaultOperationOptions)
                    .Should()
                    .BeSuccessfulResult());
        }

        [Test]
        public void SuccessOnAnyRefreshAsync()
        {
            Container
                .EffectAsync(async c =>
                {
                    var result = await c.GetRequiredService<ICacheStore<InVoid>>()
                        .RefreshAsync(Key, DefaultOperationOptions);
                    result
                        .Should()
                        .BeSuccessfulResult();
                });
        }

        [Test]
        public void SuccessOnAnyRemove()
        {
            Container
                .Effect(c => c.GetRequiredService<ICacheStore<InVoid>>()
                    .Remove(Key, DefaultOperationOptions)
                    .Should()
                    .BeSuccessfulResult());
        }

        [Test]
        public void SuccessOnAnyRemoveAsync()
        {
            Container
                .EffectAsync(async c =>
                {
                    var result = await c.GetRequiredService<ICacheStore<InVoid>>()
                        .RemoveAsync(Key, DefaultOperationOptions);
                    result
                        .Should()
                        .BeSuccessfulResult();
                });
        }
    }
}