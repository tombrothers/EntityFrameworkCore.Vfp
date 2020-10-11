using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpSqlTranslatingExpressionVisitor : RelationalSqlTranslatingExpressionVisitor {
        public VfpSqlTranslatingExpressionVisitor(
            [NotNull] RelationalSqlTranslatingExpressionVisitorDependencies dependencies,
            [NotNull] QueryCompilationContext queryCompilationContext,
            [NotNull] QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor
        ) : base(dependencies, queryCompilationContext, queryableMethodTranslatingExpressionVisitor) {
        }
    }
}
