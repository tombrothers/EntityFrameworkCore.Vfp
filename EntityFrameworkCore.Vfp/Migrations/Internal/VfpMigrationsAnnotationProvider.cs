using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Migrations.Internal {
    public class VfpMigrationsAnnotationProvider : MigrationsAnnotationProvider {
        public VfpMigrationsAnnotationProvider([NotNull] MigrationsAnnotationProviderDependencies dependencies) : base(dependencies) {
        }
    }
}
