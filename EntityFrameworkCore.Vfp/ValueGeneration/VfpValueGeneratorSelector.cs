using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.ValueGeneration {
    public class VfpValueGeneratorSelector : RelationalValueGeneratorSelector {
        public VfpValueGeneratorSelector([NotNull] ValueGeneratorSelectorDependencies dependencies) : base(dependencies) {
        }
    }
}
