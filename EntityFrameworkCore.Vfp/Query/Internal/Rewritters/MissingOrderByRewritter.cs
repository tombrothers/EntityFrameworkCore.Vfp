using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFrameworkCore.Vfp.Query.Internal.Rewritters {
    public class MissingOrderByRewritter : ExpressionVisitor {
        private static readonly ConstructorInfo ColumnExpressionConstuctorInfo;

        static MissingOrderByRewritter() {
            var type = typeof(ColumnExpression);
            var paramTypes = new[] { typeof(IProperty), typeof(IColumnBase), typeof(TableExpressionBase), typeof(bool) };

            ColumnExpressionConstuctorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, paramTypes, null);
        }

        protected override Expression VisitExtension(Expression expression) {
            if(expression is ShapedQueryExpression shapedQueryExpression) {
                if(shapedQueryExpression.QueryExpression is SelectExpression selectExpression &&
                    selectExpression.Limit != null &&
                    !selectExpression.Orderings.Any() &&
                    selectExpression.Tables.Count() == 1 &&
                    shapedQueryExpression.ShaperExpression is EntityShaperExpression entityShaperExpression &&
                    entityShaperExpression.EntityType.FindPrimaryKey() != null &&
                    selectExpression.Tables.Single() is TableExpression tableExpression
                ) {
                    var entityType = entityShaperExpression.EntityType;
                    var table = entityType.GetViewOrTableMappings().Single().Table;
                    var propertyExpressions = new Dictionary<IProperty, ColumnExpression>();

                    foreach(var property in entityType
                        .GetAllBaseTypes().Concat(entityType.GetDerivedTypesInclusive())
                        .SelectMany(Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDeclaredProperties)) {

                        var columnExpression = CreateColumnExpression(property, table.FindColumn(property), tableExpression, false);

                        propertyExpressions[property] = columnExpression;
                    }

                    var entityProjectionExpression = new EntityProjectionExpression(entityType, propertyExpressions, null);

                    foreach(var property in entityType.FindPrimaryKey().Properties) {
                        var columnExpression = entityProjectionExpression.BindProperty(property);

                        selectExpression.AppendOrdering(new OrderingExpression(columnExpression, true));
                    }
                }

                return shapedQueryExpression.Update(Visit(shapedQueryExpression.QueryExpression), shapedQueryExpression.ShaperExpression);
            }

            return base.VisitExtension(expression);
        }

        public static ColumnExpression CreateColumnExpression(IProperty property, IColumnBase column, TableExpressionBase table, bool nullable) {
            var paramValues = new object[] { property, column, table, nullable };

            return (ColumnExpression)ColumnExpressionConstuctorInfo.Invoke(paramValues);
        }
    }
}
