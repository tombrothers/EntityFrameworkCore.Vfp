using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.Vfp.Storage {
    public class VfpTypeMapping : RelationalTypeMapping {
        public VfpTypeMapping(
            [NotNull] string storeType,
            Type type,
            DbType? dbType = null)
            : base(storeType, type, dbType) {
        }

        protected VfpTypeMapping(RelationalTypeMappingParameters parameters)
            : base(parameters) {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) => new VfpTypeMapping(parameters);
    }
}
