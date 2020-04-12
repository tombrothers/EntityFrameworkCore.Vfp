using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Vfp.Query.Internal.Rewritters {
    public class MissingOrderByRewritter : ExpressionVisitor {
        protected override Expression VisitExtension(Expression expression) {
            if(expression is ShapedQueryExpression shapedQueryExpression) {
                if(shapedQueryExpression.QueryExpression is SelectExpression selectExpression &&
                    selectExpression.Limit != null &&
                    !selectExpression.Orderings.Any() &&
                    selectExpression.Tables.Count() == 1 &&
                    shapedQueryExpression.ShaperExpression is EntityShaperExpression entityShaperExpression &&
                    entityShaperExpression.EntityType.FindPrimaryKey() != null
                ) {
                    var entityProjectionExpression = new EntityProjectionExpression(entityShaperExpression.EntityType, selectExpression.Tables.Single(), false);

                    foreach(var property in entityShaperExpression.EntityType.FindPrimaryKey().Properties) {
                        var columnExpression = entityProjectionExpression.BindProperty(property);

                        selectExpression.AppendOrdering(new OrderingExpression(columnExpression, true));
                    }
                }

                return shapedQueryExpression.Update(Visit(shapedQueryExpression.QueryExpression), shapedQueryExpression.ShaperExpression);
            }

            return base.VisitExtension(expression);
        }
    }
}
