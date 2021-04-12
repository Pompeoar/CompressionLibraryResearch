using CompressionLibraryResearch;
using CompressionLibraryResearchTests;
using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;


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
            var sut = new CompressionBenchmarks();
            for (var i = 0; i < 10; i++)
            {
                sut.DotNetZip_CompressLargeFileBenchmark();

            }
        }

        [Fact]
        public async Task SmallFileBenchmarkWorks()
        {
            var sut = new CompressionBenchmarks();
            for (var i = 0; i < 10; i++)
            {
                sut.DotNetZip_CompressSmallFileBenchmark();
            }
        }
    }
}
