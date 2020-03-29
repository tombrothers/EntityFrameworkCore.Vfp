using Microsoft.EntityFrameworkCore.Update;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using EntityFrameworkCore.Vfp.Update.Internal.Interfaces;

namespace EntityFrameworkCore.Vfp.Update.Internal {
    public class VfpUpdateSqlGenerator : UpdateSqlGenerator, IVfpUpdateSqlGenerator {
        public VfpUpdateSqlGenerator([NotNull] UpdateSqlGeneratorDependencies dependencies) : base(dependencies) {
        }

        protected override void AppendIdentityWhereCondition([NotNull] StringBuilder commandStringBuilder, [NotNull] ColumnModification columnModification) {
            throw new System.NotImplementedException();
        }

        protected override void AppendRowsAffectedWhereCondition([NotNull] StringBuilder commandStringBuilder, int expectedRowsAffected) {
            throw new System.NotImplementedException();
        }
    }
}
