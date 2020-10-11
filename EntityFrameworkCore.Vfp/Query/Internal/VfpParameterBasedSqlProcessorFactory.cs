using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory {
        private readonly RelationalParameterBasedSqlProcessorDependencies _dependencies;

        public VfpParameterBasedSqlProcessorFactory(
            [NotNull] RelationalParameterBasedSqlProcessorDependencies dependencies) {

            _dependencies = dependencies.ThrowIfNull(nameof(dependencies));
        }

        public virtual RelationalParameterBasedSqlProcessor Create(bool useRelationalNulls)
            => new VfpParameterBasedSqlProcessor(_dependencies, useRelationalNulls);
    }
}
