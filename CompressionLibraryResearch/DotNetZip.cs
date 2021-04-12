using System.IO;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Ionic.Zip;

namespace ZipAndEncrypt
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class DotNetZip
    {
        public static void CompressFile(string source, string destination)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.MaxOutputSegmentSize = 2_000_000;
                zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                zip.Password = "supersecure";
                zip.AddFile(source, "files");
                zip.Save(destination);
            }
        }

        [Benchmark(Baseline = true)]
        public void CompressLargeFileBenchmark()
        {
            var inputDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Raw\city of towers.db");
            var outputDirectory = Path.GetTempPath();
            var outputFile = Path.Join(outputDirectory, "demo.zip"); ;

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            CompressFile(inputDirectory, outputFile);

            // Sometimes the Sfx test locks the .exe file for a few milliseconds.
            for (var n = 0; n < 10; n++)
            {
                try
                {
                    Directory.Delete(outputDirectory, true);
                    break;
                }
                catch
                {
                    Thread.Sleep(20);
                }
            }
        }

        [Benchmark]
        public void CompressSmallFileBenchmark()
        {
            var inputDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Raw\benchmark_phonebook.json");
            var outputDirectory = Path.GetTempPath();
            var destination = Path.Join(outputDirectory, "demo.zip"); ;

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            CompressFile(inputDirectory, destination);

            // Sometimes the Sfx test locks the .exe file for a few milliseconds.
            for (var n = 0; n < 10; n++)
            {
                try
                {
                    Directory.Delete(outputDirectory, true);
                    break;
                }
                catch
                {
                    Thread.Sleep(20);
                }
            }
        }


    }
}
