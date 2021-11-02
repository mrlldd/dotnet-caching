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

namespace mrlldd.Caching.Tests.Stores.Base
{
    public abstract class StoreDecoratorTest : StoreRelatedTest
    {
        [Test]
        public void CallsGet()
        {
            Container
                .Effect(c =>
                    CallsSpecific(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                        x => x.Get<VoidUnit>(It.Is<string>(s => s.Equals(Key)),
                            It.Is<ICacheStoreOperationOptions>(m => m == DefaultOperationOptions)),
                        _ => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                            .CacheStore
                            .Get<VoidUnit>(Key, DefaultOperationOptions),
                        new VoidUnit().AsSuccess()
                    )
                );
        }

        [Test]
        public Task CallsGetAsync()
        {
            return Container
                .EffectAsync(c =>
                    CallsSpecificAsync(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                        x => x.GetAsync<VoidUnit>(It.Is<string>(s => s.Equals(Key)),
                            It.Is<ICacheStoreOperationOptions>(m => m == DefaultOperationOptions),
                            It.IsAny<CancellationToken>()),
                        _ => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                            .CacheStore
                            .GetAsync<VoidUnit>(Key, DefaultOperationOptions)
                            .AsTask(),
                        new ValueTask<Result<VoidUnit>>(new VoidUnit().AsSuccess())
                    )
                );
        }

        [Test]
        public void CallsSet()
        {
            Container
                .Effect(c =>
                    CallsSpecific(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                        x => x.Set(It.Is<string>(s => s.Equals(Key)),
                            It.IsAny<VoidUnit>(),
                            It.Is<CachingOptions>(o => o == CachingOptions),
                            It.Is<ICacheStoreOperationOptions>(m => m == DefaultOperationOptions)),
                        _ => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                            .CacheStore
                            .Set(Key, new VoidUnit(), CachingOptions, DefaultOperationOptions),
                        Result.Success
                    )
                );
        }

        [Test]
        public Task CallsSetAsync()
        {
            return Container
                .EffectAsync(c =>
                    CallsSpecificAsync(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                        x => x.SetAsync(It.Is<string>(s => s.Equals(Key)),
                            It.IsAny<VoidUnit>(),
                            It.Is<CachingOptions>(o => o == CachingOptions),
                            It.Is<ICacheStoreOperationOptions>(m => m == DefaultOperationOptions),
                            It.IsAny<CancellationToken>()),
                        _ => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                            .CacheStore
                            .SetAsync(Key, new VoidUnit(), CachingOptions,
                                DefaultOperationOptions)
                            .AsTask(),
                        new ValueTask<Result>(Result.Success)
                    ));
        }

        [Test]
        public void CallsRefresh()
        {
            Container
                .Effect(c =>
                    CallsSpecific(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                        x => x.Refresh(It.Is<string>(s => s.Equals(Key)),
                            It.Is<ICacheStoreOperationOptions>(m => m == DefaultOperationOptions)),
                        _ => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                            .CacheStore
                            .Refresh(Key, DefaultOperationOptions),
                        Result.Success
                    ));
        }

        [Test]
        public Task CallsRefreshAsync()
        {
            return Container
                .EffectAsync(c =>
                    CallsSpecificAsync(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                        x => x.RefreshAsync(It.Is<string>(s => s.Equals(Key)),
                            It.Is<ICacheStoreOperationOptions>(m => m == DefaultOperationOptions),
                            It.IsAny<CancellationToken>()),
                        _ => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                            .CacheStore
                            .RefreshAsync(Key, DefaultOperationOptions)
                            .AsTask(),
                        new ValueTask<Result>(Result.Success)
                    ));
        }

        [Test]
        public void CallsRemove()
        {
            Container
                .Effect(c =>
                    CallsSpecific(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                        x => x.Remove(It.Is<string>(s => s.Equals(Key)),
                            It.Is<ICacheStoreOperationOptions>(m => m == DefaultOperationOptions)),
                        _ => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                            .CacheStore
                            .Remove(Key, DefaultOperationOptions),
                        Result.Success
                    ));
        }

        [Test]
        public void CallsRemoveAsync()
        {
            Container
                .EffectAsync(c =>
                    CallsSpecificAsync(c.GetRequiredService<Mock<ICacheStore<InVoid>>>(),
                        x => x.RemoveAsync(It.Is<string>(s => s.Equals(Key)),
                            It.Is<ICacheStoreOperationOptions>(m => m == DefaultOperationOptions),
                            It.IsAny<CancellationToken>()),
                        _ => c.GetRequiredService<ICacheStoreProvider<InVoid>>()
                            .CacheStore
                            .RemoveAsync(Key, DefaultOperationOptions)
                            .AsTask(),
                        new ValueTask<Result>(Result.Success)
                    ));
        }

        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            container.AddMock<ICacheStore<InVoid>>(MockRepository);
        }
    }
}