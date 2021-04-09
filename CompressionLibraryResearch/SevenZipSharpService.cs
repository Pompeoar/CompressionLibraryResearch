using SevenZip;
using System;
using System.IO;
using System.Reflection;

namespace ZipAndEncrypt
{
    public static class SevenZipSharpService
    {
        private static void SetSevenZipSharpBasePath()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");
            SevenZipBase.SetLibraryPath(path);
        }
        public static void CompressFile(string destination, string source)
        {
            SetSevenZipSharpBasePath();
            var compressor = new SevenZipCompressor();
            compressor.CompressFiles(destination, source);
        }

        public static void CompressDirectoryMultiVolume(string destination, string source, int volumeSize)
        {
            SetSevenZipSharpBasePath();
            var compressor = new SevenZipCompressor()
            {
                VolumeSize = volumeSize,
                CompressionMode = CompressionMode.Create
            };
            compressor.CompressDirectory(source, destination, "password");
        }

        public static void CompressFileMultiVolume(string destination, string source, int volumeSize)
        {
            SetSevenZipSharpBasePath();
            var compressor = new SevenZipCompressor()
            {
                VolumeSize = volumeSize,
                CompressionMode = CompressionMode.Create
            };
            compressor.CompressFilesEncrypted(destination, "password", source);
        }

        public static void CompressAndEncryptFileStream(string destination, string source)
        {
            SetSevenZipSharpBasePath();
            var compressor = new SevenZipCompressor()
            {
                CompressionMode = CompressionMode.Create,
                DirectoryStructure = false
            };

            using (var input = File.OpenRead(source))
            {
                using (var output = File.Create(destination))
                {
                    compressor.CompressStream(input, output, "password");
                }

            }
        }
        public static void CompressFileStreamMultiVolume(string destination, string source, int volumeSize)
        {
            SetSevenZipSharpBasePath();
            var compressor = new SevenZipCompressor()
            {
                VolumeSize = volumeSize,
                CompressionMode = CompressionMode.Create
            };

            using (var input = File.OpenRead(source))
            {
                using (var output = File.Create(destination))
                {
                    compressor.CompressStream(input, output, "password");
                }

            }           
        }
    }
}
