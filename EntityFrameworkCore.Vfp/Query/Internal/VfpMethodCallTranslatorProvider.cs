using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpMethodCallTranslatorProvider : RelationalMethodCallTranslatorProvider {
        public VfpMethodCallTranslatorProvider([NotNull] RelationalMethodCallTranslatorProviderDependencies dependencies) : base(dependencies) {
        }
    }
}
