using System.Data;
using System.Data.Common;

namespace EntityFrameworkCore.Vfp.VfpOleDb {
    public class VfpConnection : VfpClient.VfpConnection {
        public VfpConnection() {
        }

        public VfpConnection(string connectionString)
            : base(connectionString) {
        }

        protected override DbProviderFactory DbProviderFactory => VfpProviderFactory.Instance;

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) {
            if(isolationLevel > IsolationLevel.ReadUncommitted) {
                isolationLevel = IsolationLevel.ReadUncommitted;
            }

            return base.BeginDbTransaction(isolationLevel);
        }

        public new VfpCommand CreateCommand() => (VfpCommand)CreateDbCommand();

        protected override DbCommand CreateDbCommand() => new VfpCommand(OleDbConnection.CreateCommand(), this);
    }
}
