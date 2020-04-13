using EntityFrameworkCore.Vfp.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Vfp.Query.Internal.Rewritters {
    public class SingleRowTableRewritter : ExpressionVisitor {
        protected override Expression VisitExtension(Expression expression) {
            if(expression is ShapedQueryExpression shapedQueryExpression) {
                var queryExpression = shapedQueryExpression.QueryExpression;

                if(shapedQueryExpression.QueryExpression is SelectExpression selectExpression &&
                    !selectExpression.Tables.Any()
                ) {
                    queryExpression = selectExpression.Update(
                        selectExpression.Projection.ToList(),
                        new List<TableExpressionBase> { new SingleRowTableExpression() },
                        selectExpression.Predicate,
                        selectExpression.GroupBy.ToList(),
                        selectExpression.Having,
                        selectExpression.Orderings.ToList(),
                        selectExpression.Limit,
                        selectExpression.Offset,
                        selectExpression.IsDistinct,
                        selectExpression.Alias
                    );
                }

                return shapedQueryExpression.Update(Visit(queryExpression), shapedQueryExpression.ShaperExpression);
            }

            return base.VisitExtension(expression);
        }
    }
}
