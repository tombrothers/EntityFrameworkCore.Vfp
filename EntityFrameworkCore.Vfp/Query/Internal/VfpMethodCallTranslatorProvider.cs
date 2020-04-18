using EntityFrameworkCore.Vfp.Query.Internal.MethodCallTranslators;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpMethodCallTranslatorProvider : RelationalMethodCallTranslatorProvider {
        public VfpMethodCallTranslatorProvider([NotNull] RelationalMethodCallTranslatorProviderDependencies dependencies) : base(dependencies) {
            var sqlExpressionFactory = dependencies.SqlExpressionFactory;

            AddTranslators(
                new IMethodCallTranslator[]
                {
                    new VfpFunctionsMethodTranslator(dependencies),
                    new VfpStringMethodTranslator(sqlExpressionFactory),
                    new VfpMathMethodTranslator(sqlExpressionFactory),
                    new VfpDecimalMethodTranslator(sqlExpressionFactory)                    
                }); ;
        }
    }
}
