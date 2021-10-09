using System;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Loaders;

namespace mrlldd.Caching.Benchmarks
{
    public class DependencyResolvingBenchmarks : Benchmark
    {
        private readonly IServiceProvider noDecoratorsCachingServiceProvider;
        private readonly IServiceProvider perfLoggingDecoratedServiceProvider;
        private readonly IServiceProvider actionsLoggingDecoratedServiceProvider;
        private readonly IServiceProvider actionsAndPerfLoggingDecoratedServiceProvider;
        private readonly IServiceProvider memoryCacheServiceProvider;
        private readonly IServiceProvider distributedMemoryCacheServiceProvider;

        public DependencyResolvingBenchmarks()
        {
            noDecoratorsCachingServiceProvider = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingBenchmarks).Assembly)
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;

            perfLoggingDecoratedServiceProvider = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingBenchmarks).Assembly)
                .WithPerformanceLogging<InVoid>()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;

            actionsLoggingDecoratedServiceProvider = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingBenchmarks).Assembly)
                .WithActionsLogging<InVoid>()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;
            
            actionsAndPerfLoggingDecoratedServiceProvider = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingBenchmarks).Assembly)
                .WithActionsLogging<InVoid>()
                .WithPerformanceLogging<InVoid>()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;

            memoryCacheServiceProvider = new ServiceCollection()
                .AddMemoryCache()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;

            distributedMemoryCacheServiceProvider = new ServiceCollection()
                .AddDistributedMemoryCache()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;
        }

        [Benchmark]
        public void Resolve_Microsoft_MemoryCache() => memoryCacheServiceProvider.GetRequiredService<IMemoryCache>();

        [Benchmark]
        public void Resolve_Microsoft_DistributedMemoryCache() =>
            distributedMemoryCacheServiceProvider.GetRequiredService<IDistributedCache>();

        [Benchmark]
        public void Resolve_Clean_GenericCache() => noDecoratorsCachingServiceProvider.GetRequiredService<ICache<long, InVoid>>();

        [Benchmark]
        public void Resolve_Clean_GenericLoader() =>
            noDecoratorsCachingServiceProvider.GetRequiredService<ICachingLoader<long, string, InVoid>>();

        [Benchmark]
        public void Resolve_PerfLog_DecoratedGenericCache() => perfLoggingDecoratedServiceProvider.GetRequiredService<ICache<long, InVoid>>();

        [Benchmark]
        public void Resolve_PerfLog_DecoratedGenericLoader() =>
            perfLoggingDecoratedServiceProvider.GetRequiredService<ICachingLoader<long, string, InVoid>>();

        [Benchmark]
        public void Resolve_ActionsLog_DecoratedGenericCache() => actionsLoggingDecoratedServiceProvider.GetRequiredService<ICache<long, InVoid>>();

        [Benchmark]
        public void Resolve_ActionsLog_DecoratedGenericLoader() =>
            actionsLoggingDecoratedServiceProvider.GetRequiredService<ICachingLoader<long, string, InVoid>>();
        
        [Benchmark]
        public void Resolve_ActionsAndPerfLog_DecoratedGenericCache() => actionsAndPerfLoggingDecoratedServiceProvider.GetRequiredService<ICache<long, InVoid>>();

        [Benchmark]
        public void Resolve_ActionsAndPerfLog_DecoratedGenericLoader() =>
            actionsAndPerfLoggingDecoratedServiceProvider.GetRequiredService<ICachingLoader<long, string, InVoid>>();


        public class ImplementedCache : Cache<long, InVoid>
        {
            protected override CachingOptions Options => CachingOptions.Disabled;
            protected override string CacheKey => nameof(Int64);
        }

        public class ImplementedCachingLoader : CachingLoader<long, string, InVoid>
        {
            protected override CachingOptions Options => CachingOptions.Disabled;
            protected override string CacheKey => nameof(String);

            protected override string CacheKeySuffixFactory(long args)
                => args.ToString();
        }

        public class Loader : ILoader<long, string>
        {
            public Task<string> LoadAsync(long args, CancellationToken token = default)
                => Task.FromResult(args.ToString());
        }
    }
}