using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpSqlExpressionFactory : SqlExpressionFactory {
        private readonly IRelationalTypeMappingSource _typeMappingSource;

        public VfpSqlExpressionFactory([NotNull] SqlExpressionFactoryDependencies dependencies) : base(dependencies) {
            _typeMappingSource = dependencies.TypeMappingSource;
        }

        public override SqlFunctionExpression Coalesce(
            SqlExpression left,
            SqlExpression right,
            RelationalTypeMapping typeMapping = null
        ) {
            left.ThrowIfNull(nameof(left));
            right.ThrowIfNull(nameof(right));

            var resultType = right.Type;
            var inferredTypeMapping = typeMapping
                ?? ExpressionExtensions.InferTypeMapping(left, right)
                ?? _typeMappingSource.FindMapping(resultType);

            var typeMappedArguments = new List<SqlExpression>()
            {
                ApplyTypeMapping(left, inferredTypeMapping),
                ApplyTypeMapping(right, inferredTypeMapping)
            };

            return new SqlFunctionExpression(
                null,
                "NVL",
                typeMappedArguments,
                true,
                new[] { false, false },
                resultType,
                inferredTypeMapping
            );
        }
    }
}
