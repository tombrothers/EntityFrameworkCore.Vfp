using EntityFrameworkCore.Vfp.Query.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpQueryTranslationPostprocessor : RelationalQueryTranslationPostprocessor {
        private QueryCompilationContext _queryCompilationContext;

        public VfpQueryTranslationPostprocessor(
            [NotNull] QueryTranslationPostprocessorDependencies dependencies,
            [NotNull] RelationalQueryTranslationPostprocessorDependencies relationalDependencies,
            [NotNull] QueryCompilationContext queryCompilationContext
        ) : base(dependencies, relationalDependencies, queryCompilationContext) {
            _queryCompilationContext = queryCompilationContext;
        }

        public override Expression Process(Expression query) {
            query = base.Process(query);
            query = new MissingOrderByRewritterExpressionVisitor().Visit(query);

            return query;
        }
    }
}
