using EntityFrameworkCore.Vfp.VfpOleDb;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static System.String;

namespace EntityFrameworkCore.Vfp.Storage.Internal {
    public class VfpSqlGenerationHelper : RelationalSqlGenerationHelper {
        public override string StatementTerminator => VfpCommand.SplitCommandsToken;

        public VfpSqlGenerationHelper([NotNull] RelationalSqlGenerationHelperDependencies dependencies) : base(dependencies) {
        }

        public override void DelimitIdentifier(StringBuilder builder, string identifier) {
            identifier.ThrowIfNullOrEmpty(nameof(identifier));

            EscapeIdentifier(builder, identifier);
        }

        public override string DelimitIdentifier(string identifier) => $"{EscapeIdentifier(identifier.ThrowIfNull(nameof(identifier)))}";
    }
}
