using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace EntityFrameworkCore.Vfp.Query.Internal.MethodCallTranslators {
    public class VfpFunctionsMethodTranslator : IMethodCallTranslator {
        private static readonly Dictionary<MethodInfo, string> _supportedMethodTranslations = typeof(VfpFunctions)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .ToDictionary(x => x, x => x.Name);

        private readonly RelationalMethodCallTranslatorProviderDependencies _dependencies;

        public VfpFunctionsMethodTranslator([NotNull] RelationalMethodCallTranslatorProviderDependencies dependencies) {
            _dependencies = dependencies.ThrowIfNull(nameof(dependencies));
        }

        public SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger) {
            method.ThrowIfNull(nameof(method));
            arguments.ThrowIfNull(nameof(arguments));
            logger.ThrowIfNull(nameof(logger));

            if(_supportedMethodTranslations.TryGetValue(method, out var sqlFunctionName)) {
                var typeMappings = arguments.Select(x => ExpressionExtensions.InferTypeMapping(x)).ToArray();
                var newArguments = arguments.Select((argument, index) => _dependencies.SqlExpressionFactory.ApplyTypeMapping(argument, typeMappings[index])).ToArray();

                return _dependencies.SqlExpressionFactory.Function(
                    sqlFunctionName,
                    newArguments,
                    true,
                    newArguments.Select(a => true).ToArray(),
                    method.ReturnType,
                   _dependencies.RelationalTypeMappingSource.FindMapping(method.ReturnType));
            }

            return null;
        }
    }
}
