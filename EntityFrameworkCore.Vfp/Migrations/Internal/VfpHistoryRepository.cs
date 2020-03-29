using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Migrations.Internal {
    public class VfpHistoryRepository : HistoryRepository {
        public VfpHistoryRepository([NotNull] HistoryRepositoryDependencies dependencies) : base(dependencies) {
        }

        protected override string ExistsSql => throw new System.NotImplementedException();

        public override string GetBeginIfExistsScript(string migrationId) {
            throw new System.NotImplementedException();
        }

        public override string GetBeginIfNotExistsScript(string migrationId) {
            throw new System.NotImplementedException();
        }

        public override string GetCreateIfNotExistsScript() {
            throw new System.NotImplementedException();
        }

        public override string GetEndIfScript() {
            throw new System.NotImplementedException();
        }

        protected override bool InterpretExistsResult([NotNull] object value) {
            throw new System.NotImplementedException();
        }
    }
}
