using BenchmarkDotNet.Running;

namespace mrlldd.Caching.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Benchmark).Assembly).Run(args);
        }
    }
}