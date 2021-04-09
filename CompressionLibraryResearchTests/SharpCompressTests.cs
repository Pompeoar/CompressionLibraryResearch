using FluentAssertions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using ZipAndEncrypt;

namespace ZipAndEncryptTests
{
    public class SharpCompressTests : IDisposable
    {
        private string smallFile { get; }
        private string largeFile { get; }
        private int volumeSize = 2_000_000;
        private bool cleanupAfterTests = true;
        private bool useTempFolder = false;
        private string outputDirectory { get; }
        private string outputZipFile { get; }

        private string inputDirectory { get; }
        public SharpCompressTests() 
        {
            inputDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Raw");
            outputDirectory = useTempFolder
                ? Path.Combine(Path.GetTempPath(), "output")
                : Path.Combine(Directory.GetCurrentDirectory(), "output");
            outputZipFile = Path.Join(outputDirectory, "temp.tgz");
            Directory.CreateDirectory(outputDirectory);
            smallFile = Path.Join(inputDirectory, "benchmark_phonebook.json");
            largeFile = Path.Join(inputDirectory, "city of towers.db");
        }

        [Fact]
        public async Task CompressFileDirectory()
        {

            // Arrange

            // Act
            SharpCompressService.CompressFilesInDirectory(inputDirectory, outputZipFile);

            //Assert
            new FileInfo(outputZipFile).Exists
                .Should()
                .BeTrue();          
        }

        [Fact]
        public async Task CompressFile()
        {

            // Arrange

            // Act
            SharpCompressService.CompressFile(largeFile, outputZipFile);

            //Assert
            var fileInfo = new FileInfo(outputZipFile);
            fileInfo.Exists
                .Should()
                .BeTrue();

            fileInfo.Length
                .Should()
                .BeGreaterThan(1)
                .And
                .BeLessThan(new FileInfo(largeFile).Length);
                 
        }

        public void Dispose()
        {
           if(cleanupAfterTests && Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
        }
    }
}
