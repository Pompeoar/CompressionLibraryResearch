using BenchmarkDotNet.Running;

namespace CompressionLibraryResearch
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CompressionBenchmarks>();            
        }
    }
}
