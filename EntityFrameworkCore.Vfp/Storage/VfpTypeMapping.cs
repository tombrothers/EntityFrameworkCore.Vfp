using Microsoft.EntityFrameworkCore.Storage;
using System;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Storage {
    public class VfpTypeMapping : RelationalTypeMapping {
        private readonly VfpType _vfpType;

        public VfpTypeMapping(
            VfpType vfpType,
            Type type
        ) : this(new RelationalTypeMappingParameters(
                new CoreTypeMappingParameters(type),
                vfpType.ToVfpTypeName(),
                StoreTypePostfix.None,
                vfpType.ToDbType()
            ),
            vfpType
        ) {
        }

        protected VfpTypeMapping(RelationalTypeMappingParameters parameters, VfpType vfpType)
            : base(parameters) {
            _vfpType = vfpType;
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) => new VfpTypeMapping(parameters, _vfpType);
    }
}
