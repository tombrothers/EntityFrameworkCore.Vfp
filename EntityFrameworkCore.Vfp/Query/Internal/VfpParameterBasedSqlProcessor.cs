using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpParameterBasedSqlProcessor : RelationalParameterBasedSqlProcessor {
        public VfpParameterBasedSqlProcessor([NotNull] RelationalParameterBasedSqlProcessorDependencies dependencies, bool useRelationalNulls) : base(dependencies, useRelationalNulls) {
        }

        protected override SelectExpression ProcessSqlNullability(
            [NotNull] SelectExpression selectExpression,
            [NotNull] IReadOnlyDictionary<string, object> parametersValues,
            out bool canCache) {
            selectExpression.ThrowIfNull(nameof(selectExpression));
            parametersValues.ThrowIfNull(nameof(parametersValues));

            return new VfpSqlNullabilityProcessor(Dependencies, UseRelationalNulls).Process(selectExpression, parametersValues, out canCache);
        }
    }
}
