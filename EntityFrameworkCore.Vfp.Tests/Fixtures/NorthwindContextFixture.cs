using EntityFrameworkCore.Vfp.Tests.Data.Northwind;
using System.IO;
using System.IO.Compression;

namespace EntityFrameworkCore.Vfp.Tests.Fixtures {
    public class NorthwindContextFixture : DbContextFixtureBase<NorthwindContext> {
        private static readonly string zipFullPath;

        static NorthwindContextFixture() {
            zipFullPath = Path.Combine(RootDirectory, "NorthwindVfp.zip");
        }

        public NorthwindContextFixture() {
            EnsureZipFileExists(zipFullPath, Properties.Resources.NorthwindVfpZip);

            ZipFile.ExtractToDirectory(zipFullPath, this.DataDirectory);
        }

        protected override NorthwindContext CreateContext() => new NorthwindContext(Path.Combine(this.DataDirectory, "northwind.dbc"));
    }
}
