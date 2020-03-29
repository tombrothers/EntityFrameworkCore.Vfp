using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Migrations {
    public class VfpMigrationsSqlGenerator : MigrationsSqlGenerator {
        public VfpMigrationsSqlGenerator([NotNull] MigrationsSqlGeneratorDependencies dependencies) : base(dependencies) {
        }
    }
}
