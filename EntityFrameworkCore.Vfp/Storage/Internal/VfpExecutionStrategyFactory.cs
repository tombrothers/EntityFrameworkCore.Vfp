using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Storage.Internal {
    public class VfpExecutionStrategyFactory : RelationalExecutionStrategyFactory {
        public VfpExecutionStrategyFactory([NotNull] ExecutionStrategyDependencies dependencies) : base(dependencies) {
        }
    }
}
