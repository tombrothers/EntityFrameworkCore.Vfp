using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Storage.Internal {
    public class VfpDatabaseCreator : RelationalDatabaseCreator {
        public VfpDatabaseCreator([NotNull] RelationalDatabaseCreatorDependencies dependencies) : base(dependencies) {
        }

        public override void Create() {
            throw new System.NotImplementedException();
        }

        public override void Delete() {
            throw new System.NotImplementedException();
        }

        public override bool Exists() {
            throw new System.NotImplementedException();
        }

        public override bool HasTables() {
            throw new System.NotImplementedException();
        }
    }
}
