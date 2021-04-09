using CompressionLibraryResearchTests;
using FluentAssertions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using ZipAndEncrypt;

namespace ZipAndEncryptTests
{
    public class SharpCompressTests : TestBase
    {
        [Fact]
        public async Task CompressFileDirectory()
        {

            // Arrange

            // Act
            SharpCompressService.CompressFilesInDirectory(InputDirectory, OutputZipFile);

            //Assert
            new FileInfo(OutputZipFile).Exists
                .Should()
                .BeTrue();          
        }

        [Fact]
        public async Task CompressFile()
        {

            // Arrange

            // Act
            SharpCompressService.CompressFile(LargeFile, OutputZipFile);

            //Assert
            var fileInfo = new FileInfo(OutputZipFile);
            fileInfo.Exists
                .Should()
                .BeTrue();

            fileInfo.Length
                .Should()
                .BeGreaterThan(1)
                .And
                .BeLessThan(new FileInfo(LargeFile).Length);
                 
        }
    }
}
