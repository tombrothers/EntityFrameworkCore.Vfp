using EntityFrameworkCore.Vfp.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpQuerySqlGenerator : QuerySqlGenerator {
        protected override string AliasSeparator { get; } = " ";

        public VfpQuerySqlGenerator([NotNull] QuerySqlGeneratorDependencies dependencies) : base(dependencies) {
        }

        protected override Expression VisitExtension(Expression expression) {
            if(expression is SingleRowTableExpression singleRowTableExpression) {
                return VisitSingleRowTable(singleRowTableExpression);
            }

            return base.VisitExtension(expression);
        }

        private Expression VisitSingleRowTable(SingleRowTableExpression singleRowTableExpression) {
            Sql.Append(singleRowTableExpression.Name)
                .Append(AliasSeparator)
                .Append(singleRowTableExpression.Alias);

            return singleRowTableExpression;
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
