using Microsoft.EntityFrameworkCore.Query;
using System.Data.Common;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Query.Internal {
    public class VfpQueryStringFactory : IRelationalQueryStringFactory {
        public string Create(DbCommand command) {
            return command.CommandText;
        }
    }
}
