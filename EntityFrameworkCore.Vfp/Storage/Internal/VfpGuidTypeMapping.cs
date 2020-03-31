using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Storage.Internal {
    public class VfpGuidTypeMapping : VfpTypeMapping {
        public VfpGuidTypeMapping() : base(VfpType.Character, typeof(Guid)) {
        }

        public override DbParameter CreateParameter(
            [NotNull] DbCommand command,
            [NotNull] string name,
            [AllowNull] object value,
            bool? nullable = null
        ) => base.CreateParameter(command, name, value?.ToString(), nullable);        
    }
}
