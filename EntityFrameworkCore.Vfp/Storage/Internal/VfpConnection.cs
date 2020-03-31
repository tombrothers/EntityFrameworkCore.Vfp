using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using EntityFrameworkCore.Vfp.Storage.Internal.Interfaces;

namespace EntityFrameworkCore.Vfp.Storage.Internal {
    public class VfpConnection : RelationalConnection, IVfpConnection {
        public VfpConnection([NotNull] RelationalConnectionDependencies dependencies) : base(dependencies) {
        }

        protected override DbConnection CreateDbConnection() => new VfpOleDb.VfpConnection(ConnectionString);
    }
}
