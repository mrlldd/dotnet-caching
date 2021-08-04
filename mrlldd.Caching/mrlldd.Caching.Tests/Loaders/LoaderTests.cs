using System.Threading.Tasks;
using DryIoc;
using Functional.Object.Extensions;
using Moq;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Tests.Loaders.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Loaders
{
    public class LoaderTests : LoaderRelatedTest
    {
        [Test]
        public Task LoadsIfThereIsNothingCached() => Container
            .Map(async c =>
            {
                var argument = TestArgument.Create();
                var provider = c.Resolve<ILoaderProvider>();
                var loader = provider.Get<TestArgument, TestUnit>();
                await loader.GetOrLoadAsync(argument);
                c.Resolve<Mock<ITestClient>>()
                    .Verify(x => x.LoadAsync(It.IsAny<TestArgument>()), Times.Once);
            });


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