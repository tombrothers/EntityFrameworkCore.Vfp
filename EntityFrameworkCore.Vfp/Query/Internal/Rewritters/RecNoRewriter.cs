using EntityFrameworkCore.Vfp.Storage;
using EntityFrameworkCore.Vfp.Storage.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq;
using System.Linq.Expressions;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Query.Internal.Rewritters {
    public class RecNoRewriter : ExpressionVisitor {
        protected override Expression VisitExtension(Expression expression) {
            if(expression is ShapedQueryExpression shapedQueryExpression) {
                return shapedQueryExpression.Update(Visit(shapedQueryExpression.QueryExpression), shapedQueryExpression.ShaperExpression);
            }

            if(expression is SelectExpression selectExpression && selectExpression.Offset != null) {
                var predicate = new SqlBinaryExpression(
                    ExpressionType.GreaterThan,
                    new SqlConstantExpression(Expression.Constant("recno()"), new VfpIntegerTypeMapping()),
                    selectExpression.Offset,
                    typeof(bool),
                    new VfpTypeMapping(VfpType.Logical, typeof(bool))
                );

                if(selectExpression.Predicate != null) {
                    predicate = new SqlBinaryExpression(
                        ExpressionType.AndAlso,
                        selectExpression.Predicate,
                        predicate,
                        typeof(bool),
                        new VfpTypeMapping(VfpType.Logical, typeof(bool))
                    );
                }

                selectExpression = selectExpression.Update(
                        selectExpression.Projection.ToList(),
                        selectExpression.Tables.ToList(),
                        predicate,
                        selectExpression.GroupBy.ToList(),
                        selectExpression.Having,
                        selectExpression.Orderings.ToList(),
                        selectExpression.Limit,
                        null,
                        selectExpression.IsDistinct,
                        selectExpression.Alias
                );

                return Visit(selectExpression);
            }

            return base.VisitExtension(expression);
        }
    }
}
