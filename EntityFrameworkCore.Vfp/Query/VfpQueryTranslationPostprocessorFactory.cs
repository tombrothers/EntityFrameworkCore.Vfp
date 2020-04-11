using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpQueryTranslationPostprocessorFactory : IQueryTranslationPostprocessorFactory {
        private readonly QueryTranslationPostprocessorDependencies _dependencies;
        private readonly RelationalQueryTranslationPostprocessorDependencies _relationalDependencies;

        public VfpQueryTranslationPostprocessorFactory(
            [NotNull] QueryTranslationPostprocessorDependencies dependencies,
            [NotNull] RelationalQueryTranslationPostprocessorDependencies relationalDependencies) {
            _dependencies = dependencies;
            _relationalDependencies = relationalDependencies;
        }

        public QueryTranslationPostprocessor Create(QueryCompilationContext queryCompilationContext) {
            queryCompilationContext.ThrowIfNull(nameof(queryCompilationContext));

            return new VfpQueryTranslationPostprocessor(
                _dependencies,
                _relationalDependencies,
                queryCompilationContext
            );
        }
    }
}
