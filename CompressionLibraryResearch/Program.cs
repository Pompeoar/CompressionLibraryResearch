using BenchmarkDotNet.Running;

namespace CompressionLibraryResearch
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<DotNetZip>();
            BenchmarkRunner.Run<SevenZipSharpService>();
        }
    }
}
