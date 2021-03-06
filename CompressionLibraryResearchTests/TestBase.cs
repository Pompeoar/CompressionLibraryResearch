using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CompressionLibraryResearchTests
{
    public abstract class TestBase : IDisposable
    {
        internal string SmallFile { get; }
        internal string LargeFile { get; }
        internal int VolumeSize = 2_000_000;
        internal bool CleanupAfterTests = false;
        internal bool useTempFolder = false;
        internal string OutputDirectory { get; }
        internal string OutputZipFile { get; }

        internal string InputDirectory { get; }

        public TestBase()
        {
            InputDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Raw");
            OutputDirectory = useTempFolder
                ? Path.Combine(Path.GetTempPath(), "output")
                : Path.Combine(Directory.GetCurrentDirectory(), "output");
            OutputZipFile = Path.Join(OutputDirectory, "temp.tgz");
            SmallFile = Path.Join(InputDirectory, "benchmark_phonebook.json");
            LargeFile = Path.Join(InputDirectory, "city of towers.db");
            SetupOutputDirectory();

        }

        private void SetupOutputDirectory()
        {
            // Sometimes we don't clean up while debugging. This ensures a fresh directory. 
            if (!CleanupAfterTests && Directory.Exists(OutputDirectory))
            {
                Directory.Delete(OutputDirectory, true);
            }
            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }
        }

        public void Dispose()
        {
            if (CleanupAfterTests && Directory.Exists(OutputDirectory))
            {

                // Sometimes the Sfx test locks the .exe file for a few milliseconds.
                for (var n = 0; n < 10; n++)
                {
                    try
                    {
                        Directory.Delete(OutputDirectory, true);
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(20);
                    }
                }
            }
        }
    }
}
