using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Metadata.Conventions {
    public class VfpConventionSetBuilder : RelationalConventionSetBuilder {
        public VfpConventionSetBuilder([NotNull] ProviderConventionSetBuilderDependencies dependencies, [NotNull] RelationalConventionSetBuilderDependencies relationalDependencies) : base(dependencies, relationalDependencies) {
        }
    }
}
