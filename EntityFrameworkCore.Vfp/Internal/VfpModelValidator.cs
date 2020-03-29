using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Internal {
    public class VfpModelValidator : RelationalModelValidator {
        public VfpModelValidator([NotNull] ModelValidatorDependencies dependencies, [NotNull] RelationalModelValidatorDependencies relationalDependencies) : base(dependencies, relationalDependencies) {
        }
    }
}
