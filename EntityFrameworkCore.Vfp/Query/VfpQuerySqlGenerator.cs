using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpQuerySqlGenerator : QuerySqlGenerator {
        public VfpQuerySqlGenerator([NotNull] QuerySqlGeneratorDependencies dependencies) : base(dependencies) {
        }

        protected override Expression VisitSelect(SelectExpression selectExpression) {
            return base.VisitSelect(selectExpression);
        }

        protected override void GenerateTop(SelectExpression selectExpression) {
            if(selectExpression.Limit != null && selectExpression.Offset == null) {
                Sql.Append("TOP ");

                Visit(selectExpression.Limit);

                Sql.Append(" ");
            }
        }

        protected override void GenerateLimitOffset([NotNull] SelectExpression selectExpression) {
        }
    }
}
