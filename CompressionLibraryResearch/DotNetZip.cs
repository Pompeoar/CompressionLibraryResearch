using System.IO;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Ionic.Zip;

namespace CompressionLibraryResearch
{ 
    public class DotNetZip
    {
        public static void CompressFile(string source, string destination)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.MaxOutputSegmentSize = 2_000_000;
                zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                zip.Password = "supersecure";
                zip.AddFile(source, "files");
                zip.Save(destination);
            }
        }
    }
}
