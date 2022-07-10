using System.Text;
using BenchmarkDotNet.Attributes;
using WordsInTextCounting.TextProcessing;

namespace Benchmark
{
    [MemoryDiagnoser]
    [RankColumn]
    public class WordCounterBenchmark
    {
        private readonly string text;

        public WordCounterBenchmark()
        {
            string path = @"Example.txt";
            text = File.ReadAllText(path, Encoding.UTF8);
        }

        [Benchmark]
        public void WordCountSingleThreaded()
        {
            TextProcessor.CountWords(text, false);
        }

        [Benchmark]
        public void WordCountMultithreaded()
        {
            TextProcessor.CountWords(text, true);
        }
    }
}
