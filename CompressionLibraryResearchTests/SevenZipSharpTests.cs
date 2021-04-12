using CompressionLibraryResearchTests;
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
    public class SevenZipSharpTests : TestBase
    { 

        [Fact]
        public async Task ZipAndSave()
        {
            // Arrange
            var unzippedFile = Path.Combine(InputDirectory, LargeFile);
            if (File.Exists(OutputZipFile))
            {
                File.Delete(OutputZipFile);
            }

            // Act
            SevenZipSharpService.CompressFile(OutputZipFile, unzippedFile);

            File.Exists(OutputZipFile)
                .Should()
                .BeTrue();

            var originalSize = new FileInfo(unzippedFile).Length;
            new FileInfo(OutputZipFile).Length
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
            SevenZipSharpService.CompressDirectoryMultiVolume(OutputZipFile, InputDirectory, VolumeSize);

            // Assert
            // Should split Multi-Volume            
            Directory.GetFiles(OutputDirectory).Length
                .Should()
                .BeGreaterThan(Directory.GetFiles(InputDirectory).Length);

            var newVolumes = new DirectoryInfo(OutputDirectory).EnumerateFiles();
            foreach (var volume in newVolumes)
            {
                volume.Length
                    .Should()
                    .BeLessOrEqualTo(VolumeSize);
            }
            // Should Compress
            var originalSize = new DirectoryInfo(InputDirectory)
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
            var unzippedFile = Path.Combine(InputDirectory, LargeFile);

            // Act
            SevenZipSharpService.CompressFileMultiVolume(OutputZipFile, unzippedFile, VolumeSize);

            // Assert
            // Should split Multi - Volume
            Directory.GetFiles(OutputDirectory).Length
                .Should()
                .BeGreaterThan(1);

            var newVolumes = new DirectoryInfo(OutputDirectory).EnumerateFiles();
            foreach (var volume in newVolumes)
            {
                volume.Length
                    .Should()
                    .BeLessOrEqualTo(VolumeSize);
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
            var unzippedFile = Path.Combine(InputDirectory, LargeFile);

            // Act
            SevenZipSharpService.CompressAndEncryptFileStream(OutputZipFile, unzippedFile);

            // Assert           
            File.Exists(OutputZipFile)
             .Should()
             .BeTrue();
        }

        [Fact]
        public async Task ZipAndEncryptFileStreamByVolume()
        {
            // Arrange
            var unzippedFile = Path.Combine(InputDirectory, LargeFile);

            // Act
            SevenZipSharpService.CompressFileStreamMultiVolume(OutputZipFile, unzippedFile, VolumeSize);

            // Assert           
            // Should split Multi - Volume
            Directory.GetFiles(OutputDirectory).Length
                .Should()
                .BeGreaterThan(1);
        }

        [Fact]
        public async Task ZipAndEncryptFileStreamDictionaryByVolume()
        {
            // Arrange            
            var sourceFiles = Directory.GetFiles(InputDirectory);

            // Act
            SevenZipSharpService.CompressDictionary(OutputZipFile, sourceFiles, VolumeSize);

            // Assert           
            // Should split Multi - Volume
            var newFiles = Directory.GetFiles(OutputDirectory);
            newFiles.Length
                .Should()
                .BeGreaterThan(sourceFiles.Length);
            var newFilesInfo = newFiles.Select(file => new FileInfo(file)); 
            var originalSize = sourceFiles.Select(file => new FileInfo(file)).Sum(file => file.Length);
            newFilesInfo.Sum(file => file.Length)
                .Should()
                .BeLessThan(originalSize);
            foreach (var newfile in newFilesInfo)
            {
                
                newfile.Length
                    .Should()
                    .BeLessOrEqualTo(VolumeSize);
            }

        }

    }
}
