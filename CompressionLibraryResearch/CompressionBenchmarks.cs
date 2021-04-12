using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace CompressionLibraryResearch
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CompressionBenchmarks
    {
        private static void SetSevenZipSharpBasePath()
        {
            var path = Path.Combine(@"C:\Dev\CompressionLibraryResearch\CompressionLibraryResearch\bin\Release\netcoreapp2.1\x64", "7z.dll");            
            SevenZipBase.SetLibraryPath(path);
        }

        [Benchmark]
        public void DotNetZip_CompressLargeFile()
        {
            var inputDirectory = @"C:\Dev\CompressionLibraryResearch\CompressionLibraryResearchTests\Data\Raw\city of towers.db";
            var outputDirectory = Path.GetTempPath();
            var outputFile = Path.Join(outputDirectory, "demo.zip"); ;

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            DotNetZip.CompressFile(inputDirectory, outputFile);

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
        public void DotNetZip_CompressSmallFile()
        {
            var inputDirectory = @"C:\Dev\CompressionLibraryResearch\CompressionLibraryResearchTests\Data\Raw\benchmark_phonebook.json";
            var outputDirectory = Path.GetTempPath();
            var destination = Path.Join(outputDirectory, "demo.zip"); ;

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            DotNetZip.CompressFile(inputDirectory, destination);

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
        public void SevenSharpZip_CompressDictionary_Stream()
        {
            var volumeSize = 2_000_000;
            var sourceFiles = Directory.GetFiles(@"C:\Dev\CompressionLibraryResearch\CompressionLibraryResearchTests\Data\Raw");
            var destination = Path.GetTempPath();
            SetSevenZipSharpBasePath();
            var compressor = new SevenZipCompressor()
            {
                VolumeSize = volumeSize,
                CompressionMode = CompressionMode.Create,
                DirectoryStructure = false
            };
            var streamDictionary = new Dictionary<string, Stream>();
            foreach (var file in sourceFiles)
            {
                var info = new FileInfo(file).Name;
                streamDictionary.Add(info, File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            }

            compressor.CompressStreamDictionary(streamDictionary, destination, "password");

        }

        [Benchmark]
        public void SevenSharpZip_CompressDictionary_NoStream()
        {
            var volumeSize = 2_000_000;
            var sourceDirectory = @"C:\Dev\CompressionLibraryResearch\CompressionLibraryResearchTests\Data\Raw";
            var destination = Path.GetTempPath();
            SetSevenZipSharpBasePath();
            var compressor = new SevenZipCompressor()
            {
                VolumeSize = volumeSize,
                CompressionMode = CompressionMode.Create
            };
            compressor.CompressDirectory(sourceDirectory, destination, "password");
        }

        [Benchmark]
        public void SevenSharpZip_CompressLargeFile_NoStream()
        {
            var volumeSize = 2_000_000;
            var inputDirectory = @"C:\Dev\CompressionLibraryResearch\CompressionLibraryResearchTests\Data\Raw\city of towers.db";
            
            var destination = Path.GetTempPath();
            SetSevenZipSharpBasePath();
            var compressor = new SevenZipCompressor()
            {
                VolumeSize = volumeSize
            };
            compressor.CompressFilesEncrypted(destination, "password", inputDirectory);
        }
    }
}
