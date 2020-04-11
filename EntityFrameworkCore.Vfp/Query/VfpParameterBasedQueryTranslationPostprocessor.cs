using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpParameterBasedQueryTranslationPostprocessor : RelationalParameterBasedQueryTranslationPostprocessor {
        public VfpParameterBasedQueryTranslationPostprocessor(
            [NotNull] RelationalParameterBasedQueryTranslationPostprocessorDependencies dependencies,
            bool useRelationalNulls
        ) : base(dependencies, useRelationalNulls) {
        }
    }
}
