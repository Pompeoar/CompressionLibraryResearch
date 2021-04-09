using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;

namespace ZipAndEncrypt
{
    public class SharpCompressService
    {
        public static void ZipFile(string fileLocation, string fileDestination)
        {
            using (var archive = ZipArchive.Create())
            {
                archive.AddAllFromDirectory(fileLocation);
                archive.SaveTo(fileDestination, CompressionType.Deflate);
            }
        }
    }
}
