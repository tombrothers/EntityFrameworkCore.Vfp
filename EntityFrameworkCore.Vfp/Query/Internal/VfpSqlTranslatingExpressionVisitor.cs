using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpSqlTranslatingExpressionVisitor : RelationalSqlTranslatingExpressionVisitor {
        public VfpSqlTranslatingExpressionVisitor(
            [NotNull] RelationalSqlTranslatingExpressionVisitorDependencies dependencies,
            [NotNull] IModel model,
            [NotNull] QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor
        ) : base(dependencies, model, queryableMethodTranslatingExpressionVisitor) {
        }
    }
}
