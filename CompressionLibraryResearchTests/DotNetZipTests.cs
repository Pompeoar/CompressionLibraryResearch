using CompressionLibraryResearchTests;
using FluentAssertions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZipAndEncrypt;


namespace ZipAndEncryptTests
{
    public class DotNetZipTests : TestBase
    {
        [Fact]
        public async Task ZipAndSave()
        {
            // Arrange          
            // Act
            DotNetZip.CompressFile(SmallFile, OutputZipFile);
            File.Exists(OutputZipFile)
                .Should()
                .BeTrue();

        }

        [Fact]
        public async Task LargeFileBenchmarkWorks()
        {
            var sut = new DotNetZip();
            for (var i = 0; i < 10; i++)
            {
                sut.CompressLargeFileBenchmark();

            }
        }

        [Fact]
        public async Task SmallFileBenchmarkWorks()
        {
            var sut = new DotNetZip();
            for (var i = 0; i < 10; i++)
            {
                sut.CompressSmallFileBenchmark();

            }
        }
    }
}
