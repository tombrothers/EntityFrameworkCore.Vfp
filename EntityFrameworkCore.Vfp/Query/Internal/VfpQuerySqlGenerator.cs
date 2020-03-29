using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpQuerySqlGenerator : QuerySqlGenerator {
        public VfpQuerySqlGenerator([NotNull] QuerySqlGeneratorDependencies dependencies) : base(dependencies) {
        }

        protected override Expression VisitSelect(SelectExpression selectExpression) {
            return base.VisitSelect(selectExpression);
        }
    }
}
