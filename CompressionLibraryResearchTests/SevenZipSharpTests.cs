using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZipAndEncrypt;

namespace ZipAndEncryptTests
{
    public class SevenZipSharpTests : IDisposable
    {
        private string smallFile = "benchmark_phonebook.json";
        private string largeFile = "city of towers.db";
        private int volumeSize = 2_000_000;
        private bool cleanupAfterTests = true;
        private bool useTempFolder = false;
        public SevenZipSharpTests()
        {
            inputDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Raw");
            outputDirectory = useTempFolder 
                ? Path.Combine(Path.GetTempPath(), "output")
                : Path.Combine(Directory.GetCurrentDirectory(), "output");
            outputZipFile = Path.Join(outputDirectory, "tmp.7z");
            Directory.CreateDirectory(outputDirectory);
        }
        //C:\Dev\CompressionLibraryResearch\CompressionLibraryResearchTests\bin\Debug\netcoreapp2.1\Data\Raw
        //C:\Dev\CompressionLibraryResearch\CompressionLibraryResearchTests\bin\Debug\Data\Raw
        private string outputDirectory { get; }
        private string outputZipFile { get; }

        private string inputDirectory { get; }
        public void Dispose()
        {

            if (cleanupAfterTests && Directory.Exists(outputDirectory))
            {
                // sometimes the sfx test locks the .exe file for a few milliseconds.
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

        [Fact]
        public async Task ZipAndSave()
        {
            // Arrange
            var unzippedFile = Path.Combine(inputDirectory, largeFile);
            if (File.Exists(outputZipFile))
            {
                File.Delete(outputZipFile);
            }

            // Act
            SevenZipSharpService.CompressFile(outputZipFile, unzippedFile);

            File.Exists(outputZipFile)
                .Should()
                .BeTrue();

            var originalSize = new FileInfo(unzippedFile).Length;
            new FileInfo(outputZipFile).Length
                .Should()
                .BeGreaterThan(0)
                .And
                .BeLessThan(originalSize);
        }


        [Fact]
        public async Task ZipAndEncryptDirectoryBySplitVolume()
        {
            // Arrange
            // Act
            SevenZipSharpService.CompressDirectoryMultiVolume(outputZipFile, inputDirectory, volumeSize);

            // Assert
            // Should split Multi-Volume            
            Directory.GetFiles(outputDirectory).Length
                .Should()
                .BeGreaterThan(Directory.GetFiles(inputDirectory).Length);

            var newVolumes = new DirectoryInfo(outputDirectory).EnumerateFiles();
            foreach (var volume in newVolumes)
            {
                volume.Length
                    .Should()
                    .BeLessOrEqualTo(volumeSize);
            }
            // Should Compress
            var originalSize = new DirectoryInfo(inputDirectory)
                .EnumerateFiles()
                .Sum(file => file.Length);
            var compressedSize = newVolumes
                .Sum(file => file.Length);

            compressedSize
                .Should()
                .BeGreaterThan(0)
                .And
                .BeLessThan(originalSize);            
        }

        [Fact]
        public async Task ZipAndEncryptFileBySplitVolume()
        {
            // Arrange
            var unzippedFile = Path.Combine(inputDirectory, largeFile);

            // Act
            SevenZipSharpService.CompressFileMultiVolume(outputZipFile, unzippedFile, volumeSize);

            // Assert
            // Should split Multi - Volume
            Directory.GetFiles(outputDirectory).Length
                .Should()
                .BeGreaterThan(1);

            var newVolumes = new DirectoryInfo(outputDirectory).EnumerateFiles();
            foreach (var volume in newVolumes)
            {
                volume.Length
                    .Should()
                    .BeLessOrEqualTo(volumeSize);
            }

            // Should Compress            
            var originalSize = new FileInfo(unzippedFile).Length;
            var compressedSize = newVolumes
                .Sum(file => file.Length);
            compressedSize
                .Should()
                .BeGreaterThan(0)
                .And
                .BeLessThan(originalSize);
        }

        [Fact]
        public async Task ZipAndEncryptFileStream()
        {
            // Arrange
            var unzippedFile = Path.Combine(inputDirectory, largeFile);

            // Act
            SevenZipSharpService.CompressAndEncryptFileStream(outputZipFile, unzippedFile);

            // Assert           
            File.Exists(outputZipFile)
             .Should()
             .BeTrue();
        }

        [Fact]
        public async Task ZipAndEncryptFileStreamByVolume()
        {
            // Arrange
            var unzippedFile = Path.Combine(inputDirectory, largeFile);

            // Act
            SevenZipSharpService.CompressFileStreamMultiVolume(outputZipFile, unzippedFile, volumeSize);

            // Assert           
            // Should split Multi - Volume
            Directory.GetFiles(outputDirectory).Length
                .Should()
                .BeGreaterThan(1);
        }

    }
}
