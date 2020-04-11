using EntityFrameworkCore.Vfp.Tests.Data.AllTypes;
using System.IO;
using System.IO.Compression;

namespace EntityFrameworkCore.Vfp.Tests.Fixtures {
    public class AllTypesContextFixture : DbContextFixtureBase<AllTypesContext> {
        private static readonly string zipFullPath = Path.Combine(RootDirectory, "AllTypes.zip");

        public AllTypesContextFixture() {
            EnsureZipFileExists(zipFullPath, Properties.Resources.AllTypesZip);

            ZipFile.ExtractToDirectory(zipFullPath, this.DataDirectory);
        }

        protected override AllTypesContext CreateContext() => new AllTypesContext(Path.Combine(this.DataDirectory, "AllTypes.dbc"));
    }
}
