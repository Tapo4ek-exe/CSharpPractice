using BenchmarkDotNet.Running;

namespace Benchmark
{
    class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<WordCounterBenchmark>();
        }
    }
}