using EntityFrameworkCore.Vfp.Query.Internal.MemberTranslators;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpMemberTranslatorProvider : RelationalMemberTranslatorProvider {
        public VfpMemberTranslatorProvider([NotNull] RelationalMemberTranslatorProviderDependencies dependencies)
            : base(dependencies) {
            var sqlExpressionFactory = dependencies.SqlExpressionFactory;

            AddTranslators(
                new IMemberTranslator[]
                {
                    new VfpStringMemberTranslator(sqlExpressionFactory),
                });
        }
    }
}
