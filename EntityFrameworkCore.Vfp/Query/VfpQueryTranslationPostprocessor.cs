using EntityFrameworkCore.Vfp.Query.Internal.Rewritters;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Vfp.Query {
    public class VfpQueryTranslationPostprocessor : RelationalQueryTranslationPostprocessor {
        public VfpQueryTranslationPostprocessor(
            [NotNull] QueryTranslationPostprocessorDependencies dependencies,
            [NotNull] RelationalQueryTranslationPostprocessorDependencies relationalDependencies,
            [NotNull] QueryCompilationContext queryCompilationContext
        ) : base(dependencies, relationalDependencies, queryCompilationContext) {
        }

        public override Expression Process(Expression query) {
            query = base.Process(query);
            query = new ExistsRewritter().Visit(query);
            query = new MissingOrderByRewritter().Visit(query);
            query = new RecNoRewriter().Visit(query);
            //query = new SingleRowTableRewritter().Visit(query);

#if DEBUG
            var expressionString = query.Print();

            Debug.WriteLine(expressionString);
#endif

            return query;
        }
    }
}
