using EntityFrameworkCore.Vfp.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpQuerySqlGenerator : QuerySqlGenerator {
        protected override string AliasSeparator { get; } = " ";
        private IDictionary<ExpressionType, string> _binaryFunctions = new Dictionary<ExpressionType, string> {
            { ExpressionType.And, "BITAND" },
            { ExpressionType.Or, "BITOR" },
        };

        public VfpQuerySqlGenerator([NotNull] QuerySqlGeneratorDependencies dependencies) : base(dependencies) {
        }

        protected override Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression) {
            if(_binaryFunctions.TryGetValue(sqlBinaryExpression.OperatorType, out var function)) {
                WriteSqlBinaryFunction(function, sqlBinaryExpression);

                return sqlBinaryExpression;
            }

            return base.VisitSqlBinary(sqlBinaryExpression);
        }

        private void WriteSqlBinaryFunction(string function, SqlBinaryExpression sqlBinaryExpression) {
            Sql.Append($"{function}(");

            var requiresBrackets = RequiresBrackets(sqlBinaryExpression.Left);

            if(requiresBrackets) {
                Sql.Append("(");
            }

            Visit(sqlBinaryExpression.Left);

            if(requiresBrackets) {
                Sql.Append(")");
            }

            Sql.Append(",");

            requiresBrackets = RequiresBrackets(sqlBinaryExpression.Right);

            if(requiresBrackets) {
                Sql.Append("(");
            }

            Visit(sqlBinaryExpression.Right);

            if(requiresBrackets) {
                Sql.Append(")");
            }

            Sql.Append(")");
        }

        private static bool RequiresBrackets(SqlExpression expression) => expression is SqlBinaryExpression || expression is LikeExpression;

        protected override Expression VisitExtension(Expression expression) {
            if(expression is SingleRowTableExpression singleRowTableExpression) {
                return VisitSingleRowTable(singleRowTableExpression);
            }

            return base.VisitExtension(expression);
        }

        protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression) {
            if(string.Equals("POWER", sqlFunctionExpression.Name, StringComparison.OrdinalIgnoreCase) && sqlFunctionExpression.Arguments.Count() == 2) {
                Visit(sqlFunctionExpression.Arguments.First());
                Sql.Append("^");
                Visit(sqlFunctionExpression.Arguments.Last());

                return sqlFunctionExpression;
            }

            return base.VisitSqlFunction(sqlFunctionExpression);
        }
        private Expression VisitSingleRowTable(SingleRowTableExpression singleRowTableExpression) {
            Sql.Append(singleRowTableExpression.Name)
                .Append(AliasSeparator)
                .Append(singleRowTableExpression.Alias);

            return singleRowTableExpression;
        }

        protected override Expression VisitCase(CaseExpression caseExpression) {
            caseExpression.ThrowIfNull(nameof(caseExpression));

            Sql.Append("ICASE(");

            if(caseExpression.Operand != null) {
                Sql.Append(" ");
                Visit(caseExpression.Operand);
            }

            var first = true;

            foreach(var whenClause in caseExpression.WhenClauses) {
                if(first) {
                    first = false;
                }
                else {
                    Sql.Append(",");
                }

                Visit(whenClause.Test);
                Sql.Append(",");
                Visit(whenClause.Result);
            }

            if(caseExpression.ElseResult != null) {
                Sql.Append(",");
                Visit(caseExpression.ElseResult);
            }

            Sql.Append(")");

            return caseExpression;
        }

        protected override Expression VisitSelect(SelectExpression selectExpression) {
            return base.VisitSelect(selectExpression);
        }

        protected override void GenerateTop(SelectExpression selectExpression) {
            if(selectExpression.Limit != null && selectExpression.Offset == null) {
                Sql.Append("TOP ");

                Visit(selectExpression.Limit);

                Sql.Append(" ");

                return;
            }

            base.GenerateTop(selectExpression);
        }

        protected override void GenerateLimitOffset([NotNull] SelectExpression selectExpression) {
            base.GenerateLimitOffset(selectExpression);
        }
    }
}
