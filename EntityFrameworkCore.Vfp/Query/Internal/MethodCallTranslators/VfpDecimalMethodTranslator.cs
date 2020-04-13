using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace EntityFrameworkCore.Vfp.Query.Internal.MethodCallTranslators {
    public class VfpDecimalMethodTranslator : IMethodCallTranslator {
        private static readonly Dictionary<MethodInfo, string> _supportedMethodTranslations = new Dictionary<MethodInfo, string>
        {
            { typeof(decimal).GetRuntimeMethod(nameof(decimal.Ceiling), new[] { typeof(decimal) }), "CEILING" },
            { typeof(decimal).GetRuntimeMethod(nameof(decimal.Floor), new[] { typeof(decimal) }), "FLOOR" },
        };

        private static readonly IEnumerable<MethodInfo> _roundMethodInfos = new[]
        {
            typeof(decimal).GetRuntimeMethod(nameof(decimal.Round), new[] { typeof(decimal) }),
            typeof(decimal).GetRuntimeMethod(nameof(decimal.Round), new[] { typeof(decimal), typeof(int) }),
        };

        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public VfpDecimalMethodTranslator([NotNull] ISqlExpressionFactory sqlExpressionFactory) {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public virtual SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments) {
            method.ThrowIfNull(nameof(method));
            arguments.ThrowIfNull(nameof(arguments));

            if(_supportedMethodTranslations.TryGetValue(method, out var sqlFunctionName)) {
                var typeMapping = arguments.Count == 1
                    ? ExpressionExtensions.InferTypeMapping(arguments[0])
                    : ExpressionExtensions.InferTypeMapping(arguments[0], arguments[1]);

                var newArguments = new SqlExpression[arguments.Count];
                newArguments[0] = _sqlExpressionFactory.ApplyTypeMapping(arguments[0], typeMapping);

                if(arguments.Count == 2) {
                    newArguments[1] = _sqlExpressionFactory.ApplyTypeMapping(arguments[1], typeMapping);
                }

                return _sqlExpressionFactory.Function(
                    sqlFunctionName,
                    newArguments,
                    true,
                    newArguments.Select(a => true).ToArray(),
                    method.ReturnType,
                    typeMapping);
            }

            if(_roundMethodInfos.Contains(method)) {
                var argument = arguments[0];
                var digits = arguments.Count == 2 ? arguments[1] : _sqlExpressionFactory.Constant(0);

                return _sqlExpressionFactory.Function(
                    "ROUND",
                    new[] { argument, digits },
                    true,
                    new[] { true, true },
                    method.ReturnType,
                    argument.TypeMapping);
            }

            return null;
        }
    }
}
