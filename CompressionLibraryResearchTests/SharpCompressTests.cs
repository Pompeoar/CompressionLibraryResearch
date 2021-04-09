using FluentAssertions;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using ZipAndEncrypt;

namespace ZipAndEncryptTests
{
    public class SharpCompressTests
    {
        [Fact]
        public async Task ZipAndSave()
        {
            // Arrange
            var unzippedFile = @"C:\Dev\ZipTests\worlds-20210318-050007\worlds\debug.txt";
            var zippedFolder = @"C:\Dev\ZipTests\worlds-20210318-050007\worlds\temp.7z";

            // Act
            DotNetZip.CompressFile(zippedFolder, unzippedFile);

            //Assert
            //File.Exists(zippedFolder)
            //    .Should()
            //    .BeTrue();

            //File.Delete(zippedFolder);
        }

    }
}
