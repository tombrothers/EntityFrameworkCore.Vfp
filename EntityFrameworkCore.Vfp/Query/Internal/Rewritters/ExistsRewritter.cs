using EntityFrameworkCore.Vfp.Query.SqlExpressions;
using EntityFrameworkCore.Vfp.Storage;
using EntityFrameworkCore.Vfp.Storage.Internal.TypeMappings;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Query.Internal.Rewritters {
    public class ExistsRewritter : ExpressionVisitor {
        protected override Expression VisitExtension(Expression expression) {
            if(expression is ShapedQueryExpression shapedQueryExpression) {
                var queryExpression = shapedQueryExpression.QueryExpression;

                if(shapedQueryExpression.QueryExpression is SelectExpression selectExpression &&
                    selectExpression.Projection.Count() == 1 &&
                    selectExpression.Projection.Single().Expression is ExistsExpression existsExpression &&
                    existsExpression.Subquery is SelectExpression subquerySelectExpression) {

                    subquerySelectExpression.AddToProjection(new SqlConstantExpression(Expression.Constant("COUNT(*)"), new VfpIntegerTypeMapping()));

                    var projection = (SqlExpression)new SqlBinaryExpression(
                        ExpressionType.LessThan,
                        new SqlConstantExpression(Expression.Constant("0"), new VfpIntegerTypeMapping()),
                        new ScalarSubqueryExpression(subquerySelectExpression),
                        typeof(bool),
                        new VfpLogicalTypeMapping()
                    );

                    if(existsExpression.IsNegated) {
                        projection = new SqlUnaryExpression(ExpressionType.Not, projection, typeof(bool), new VfpLogicalTypeMapping());
                    }

                    queryExpression = selectExpression.Update(
                        new List<ProjectionExpression> { new ProjectionExpression(projection, string.Empty) },
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
