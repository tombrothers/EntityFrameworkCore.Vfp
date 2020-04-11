using EntityFrameworkCore.Vfp.Tests.Data.AutoGenId;
using System.IO;
using System.IO.Compression;

namespace EntityFrameworkCore.Vfp.Tests.Fixtures {
    public class AutoGenContextFixture : DbContextFixtureBase<AutoGenContext> {
        private static readonly string zipFullPath = Path.Combine(RootDirectory, "AutoGenId.zip");

        public AutoGenContextFixture() {
            EnsureZipFileExists(zipFullPath, Properties.Resources.AutoGenIdZip);

            ZipFile.ExtractToDirectory(zipFullPath, this.DataDirectory);
        }

        protected override AutoGenContext CreateContext() => new AutoGenContext(Path.Combine(this.DataDirectory, "AutoGenIdDb.dbc"));
    }
}
