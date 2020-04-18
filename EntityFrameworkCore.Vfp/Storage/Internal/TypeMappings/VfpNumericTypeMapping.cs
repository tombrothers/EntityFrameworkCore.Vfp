using Microsoft.EntityFrameworkCore.Storage;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Storage.Internal.TypeMappings {
    public class VfpNumericTypeMapping : RelationalTypeMapping {
        private readonly VfpType _vfpType;

        public VfpNumericTypeMapping(
            VfpType vfpType,
            int? precision = null,
            int? scale = null,
            StoreTypePostfix storeTypePostfix = StoreTypePostfix.None
        ) : this(new RelationalTypeMappingParameters(
                new CoreTypeMappingParameters(typeof(decimal)),
                vfpType.ToVfpTypeName(),
                storeTypePostfix,
                vfpType.ToDbType()
            ).WithPrecisionAndScale(precision, scale),
            vfpType
        ) {
        }

        protected VfpNumericTypeMapping(RelationalTypeMappingParameters parameters, VfpType vfpType)
            : base(parameters) {
            _vfpType = vfpType;
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) => new VfpNumericTypeMapping(parameters, _vfpType);
    }
}
