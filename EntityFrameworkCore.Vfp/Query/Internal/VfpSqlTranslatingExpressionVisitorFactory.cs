using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpSqlTranslatingExpressionVisitorFactory : IRelationalSqlTranslatingExpressionVisitorFactory {
        private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;

        public VfpSqlTranslatingExpressionVisitorFactory(
            [NotNull] RelationalSqlTranslatingExpressionVisitorDependencies dependencies) {
            _dependencies = dependencies;
        }

        public RelationalSqlTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor) =>
            new VfpSqlTranslatingExpressionVisitor(_dependencies, queryCompilationContext, queryableMethodTranslatingExpressionVisitor);
    }
}
