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
            var source = Path.Combine(Directory.GetCurrentDirectory(), @"Raw\Data");
            var directory = Path.GetTempPath();
            var destination = Path.Join(directory, "demo.zip"); ;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            CompressFile(source, destination);

            // Sometimes the Sfx test locks the .exe file for a few milliseconds.
            for (var n = 0; n < 10; n++)
            {
                try
                {
                    Directory.Delete(directory, true);
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
            var source = @"C:\Dev\ZipTests\worlds-20210318-050007\worlds\benchmark_phonebook.json";
            var directory = Path.GetTempPath();
            var destination = Path.Join(directory, "demo.zip"); ;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            CompressFile(source, destination);

            // Sometimes the Sfx test locks the .exe file for a few milliseconds.
            for (var n = 0; n < 10; n++)
            {
                try
                {
                    Directory.Delete(directory, true);
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
