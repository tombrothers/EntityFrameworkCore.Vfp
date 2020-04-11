using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpMemberTranslatorProvider : RelationalMemberTranslatorProvider {
        public VfpMemberTranslatorProvider([NotNull] RelationalMemberTranslatorProviderDependencies dependencies) : base(dependencies) {
        }
    }
}
