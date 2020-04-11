using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpMethodCallTranslatorProvider : RelationalMethodCallTranslatorProvider {
        public VfpMethodCallTranslatorProvider([NotNull] RelationalMethodCallTranslatorProviderDependencies dependencies) : base(dependencies) {
        }
    }
}
