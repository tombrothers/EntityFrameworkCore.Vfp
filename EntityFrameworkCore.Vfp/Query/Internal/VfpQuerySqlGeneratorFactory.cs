using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory {
        private readonly QuerySqlGeneratorDependencies _dependencies;

        public VfpQuerySqlGeneratorFactory([NotNull] QuerySqlGeneratorDependencies dependencies) {
            _dependencies = dependencies;
        }

        public QuerySqlGenerator Create() => new VfpQuerySqlGenerator(_dependencies);
    }
}
