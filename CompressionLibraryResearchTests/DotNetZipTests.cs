using FluentAssertions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ZipAndEncrypt;


namespace ZipAndEncryptTests
{
    public class DotNetZipTests
    {
        [Fact]
        public async Task ZipAndSave()
        {
            // Arrange
            var sourceDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Raw)");
            var sourceFile = Path.Combine(sourceDirectory, "city of towers.db");
            var directory = Path.GetTempPath();
            var zip = Path.Join(directory, "demo.zip");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Act
            DotNetZip.CompressFile(sourceFile, zip);
            File.Exists(zip)
                .Should()
                .BeTrue();

            // Sometimes the Sfx test locks the .exe file for a few milliseconds.
            if (Directory.Exists(directory))
            {
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
