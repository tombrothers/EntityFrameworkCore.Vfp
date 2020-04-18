using Microsoft.EntityFrameworkCore.Storage;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Storage.Internal.TypeMappings {
    public class VfpLogicalTypeMapping : RelationalTypeMapping {
        public VfpLogicalTypeMapping() : base(VfpType.Logical.ToVfpTypeName(), typeof(bool), VfpType.Logical.ToDbType()) {
        }

        protected VfpLogicalTypeMapping(RelationalTypeMappingParameters parameters)
            : base(parameters) {
        }

        public override string GenerateSqlLiteral(object value) {
            if((bool)value) {
                return ".t.";
            }

            return ".f.";
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) => new VfpLogicalTypeMapping(parameters);
    }
}
