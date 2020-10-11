using EntityFrameworkCore.Vfp.Storage;
using EntityFrameworkCore.Vfp.Storage.Internal.TypeMappings;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Query.Internal.Rewritters {
    public class XmlToCursorRewritter : ExpressionVisitor {
        private readonly IReadOnlyDictionary<string, object> _parametersValues;

        public XmlToCursorRewritter(IReadOnlyDictionary<string, object> parametersValues) {
            _parametersValues = parametersValues.ThrowIfNull(nameof(parametersValues));
        }

        protected override Expression VisitExtension(Expression expression) {
            if(expression is ShapedQueryExpression shapedQueryExpression) {
                return shapedQueryExpression.Update(Visit(shapedQueryExpression.QueryExpression), shapedQueryExpression.ShaperExpression);
            }

            if(expression is InExpression inExpression) {
                var parameters = _parametersValues;

                return inExpression;
            }

            return base.VisitExtension(expression);
        }
    }
}
