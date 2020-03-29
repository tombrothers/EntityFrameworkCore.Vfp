using System.IO;
using System.IO.Compression;
using System.Threading;
using EntityFrameworkCore.Vfp.Tests.Data.Northwind;

namespace EntityFrameworkCore.Vfp.Tests.Fixtures {
    public class NorthwindContextFixture : DbContextFixtureBase<NorthwindContext> {
        private static readonly string zipFullPath;

        static NorthwindContextFixture() {
            zipFullPath = Path.Combine(RootDirectory, "NorthwindVfp.zip");
        }

        public NorthwindContextFixture() {
            EnsureZipFileExists();

            ZipFile.ExtractToDirectory(zipFullPath, this.DataDirectory);
        }

        protected override NorthwindContext CreateContext() => new NorthwindContext(Path.Combine(this.DataDirectory, "northwind.dbc"));

        private static void EnsureZipFileExists() {
            const int maxAttempts = 5;
            var attempt = 0;

            while(true) {
                if(File.Exists(zipFullPath)) {
                    return;
                }

                try {
                    File.WriteAllBytes(zipFullPath, Properties.Resources.NorthwindVfpZip);
                }
                catch(IOException) {
                    if(!File.Exists(zipFullPath) && attempt == maxAttempts) {
                        throw;
                    }

                    Thread.Sleep(500);
                    attempt++;
                }
            }
        }
    }
}
