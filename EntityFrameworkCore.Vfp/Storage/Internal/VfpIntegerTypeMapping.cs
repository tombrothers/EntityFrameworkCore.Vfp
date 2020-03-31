using Microsoft.EntityFrameworkCore.Storage;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Storage.Internal {
    public class VfpIntegerTypeMapping : IntTypeMapping {
        public VfpIntegerTypeMapping() : base(VfpType.Integer.ToVfpTypeName(), VfpType.Integer.ToDbType()) {
        }
    }
}
