using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Functional.Object.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using NUnit.Framework;

namespace mrlldd.Caching.Tests
{
    public class TestBase
    {
        protected IContainer Container { get; private set; } = null!;

        protected MockRepository MockRepository { get; } = new(MockBehavior.Strict);

        [SetUp]
        protected void Setup()
        {
            Container = new Container(r => r
                .WithoutFastExpressionCompiler()
                .WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace)
                .WithTrackingDisposableTransients()
                .With(FactoryMethod.ConstructorWithResolvableArguments)
            );
            
            new ServiceCollection()
                .AddLogging(x => x.AddConsole().AddFilter(f => f >= LogLevel.Debug))
                .AddCaching(typeof(TestBase).Assembly)
                .Effect(FillCachingServiceCollection)
                .Effect(x => Container.Populate(x));
            FillContainer(Container);
            
            AfterContainerEnriching();
        }

        protected virtual void FillCachingServiceCollection(ICachingServiceCollection services)
        {
            
        }

        protected virtual void FillContainer(IContainer container)
        {
        }

        protected virtual void AfterContainerEnriching()
        {
        }
    }
}