using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using static System.String;

namespace EntityFrameworkCore.Vfp.Storage.Internal {
    public class VfpSqlGenerationHelper : RelationalSqlGenerationHelper {
        public override string StatementTerminator => Empty;

        public VfpSqlGenerationHelper([NotNull] RelationalSqlGenerationHelperDependencies dependencies) : base(dependencies) {
        }

        public override string DelimitIdentifier(string identifier) => $"{EscapeIdentifier(identifier.ThrowIfNull(nameof(identifier)))}";
    }
}
