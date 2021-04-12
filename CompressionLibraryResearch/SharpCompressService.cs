using Ionic.BZip2;
using SharpCompress.Common;
using SharpCompress.Writers;
using System.IO;

namespace CompressionLibraryResearch
{
    public class SharpCompressService
    {
        public static void CompressFilesInDirectory(string sourcePath, string fileDestination)
        {
            using (Stream stream = File.OpenWrite(fileDestination))
            using (var writer = WriterFactory.Open(stream, ArchiveType.Tar, new WriterOptions(CompressionType.GZip)))
            {
                writer.WriteAll(sourcePath, " * ", SearchOption.AllDirectories);
            }
        }

        public static void CompressFile(string sourceFile, string fileDestination)
        {
            using (var inputStream = File.OpenRead(sourceFile))
            using (Stream stream = File.OpenWrite(fileDestination))            
            using (var writer = WriterFactory.Open(stream, ArchiveType.Tar, new WriterOptions(CompressionType.GZip)))
            {
                writer.Write(sourceFile, inputStream);
            }
        }
    }
}
