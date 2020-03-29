using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpParameterBasedQueryTranslationPostprocessorFactory : IRelationalParameterBasedQueryTranslationPostprocessorFactory {
        private readonly RelationalParameterBasedQueryTranslationPostprocessorDependencies _dependencies;

        public VfpParameterBasedQueryTranslationPostprocessorFactory(
            [NotNull] RelationalParameterBasedQueryTranslationPostprocessorDependencies dependencies) {
            _dependencies = dependencies;
        }

        public virtual RelationalParameterBasedQueryTranslationPostprocessor Create(bool useRelationalNulls) => new VfpParameterBasedQueryTranslationPostprocessor(_dependencies, useRelationalNulls);
    }
}
