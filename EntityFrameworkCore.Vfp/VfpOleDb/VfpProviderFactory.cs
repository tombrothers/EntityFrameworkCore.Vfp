using System.Data.Common;
using System.Diagnostics;

namespace EntityFrameworkCore.Vfp.VfpOleDb {
    public class VfpProviderFactory : VfpClient.VfpClientFactory {
        public new static readonly VfpProviderFactory Instance = new VfpProviderFactory();

#if DEBUG
        static VfpProviderFactory() {
            VfpClient.VfpClientTracing.Tracer = new TraceSource("VfpClient", SourceLevels.All);
        }
#endif

        public override DbConnection CreateConnection() => new VfpConnection();

        public override DbCommand CreateCommand() => new VfpCommand();
    }
}
