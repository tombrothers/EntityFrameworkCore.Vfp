using EntityFrameworkCore.Vfp.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpSqlNullabilityProcessor : SqlNullabilityProcessor {
        public VfpSqlNullabilityProcessor([NotNull] RelationalParameterBasedSqlProcessorDependencies dependencies, bool useRelationalNulls) : base(dependencies, useRelationalNulls) {
        }

        protected override TableExpressionBase Visit([NotNull] TableExpressionBase tableExpressionBase) {
            switch(tableExpressionBase) {
                case SingleRowTableExpression singleRowTableExpression:
                    return singleRowTableExpression;
                default:
                    return base.Visit(tableExpressionBase);
            }
        }
    }
}
