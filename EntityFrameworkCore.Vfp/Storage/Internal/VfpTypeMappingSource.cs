using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VfpClient;
using static System.String;

namespace EntityFrameworkCore.Vfp.Storage.Internal {
    public class VfpTypeMappingSource : RelationalTypeMappingSource {
        private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;
        private readonly Dictionary<string, RelationalTypeMapping> _storeTypeMappings;

        public VfpTypeMappingSource(
            [NotNull] TypeMappingSourceDependencies dependencies,
            [NotNull] RelationalTypeMappingSourceDependencies relationalDependencies
        ) : base(dependencies, relationalDependencies) {
            var vfpTypes = Enum.GetValues(typeof(VfpType)).OfType<VfpType>();
            var typeMappings = vfpTypes.Select(x => new VfpTypeMapping(GetVfpTypeString(x), x.ToType(), x.ToDbType())).ToArray();

            _storeTypeMappings = CreateStoreTypeMappings(typeMappings);
            _clrTypeMappings = CreateClrMapMappings(typeMappings);
        }

        protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo) {
            if(!IsNullOrWhiteSpace(mappingInfo.StoreTypeName)) {
                return _storeTypeMappings[mappingInfo.StoreTypeName];
            }

            if(mappingInfo.ClrType != null) {
                return _clrTypeMappings[mappingInfo.ClrType];
            }

            return null;
        }

        private static Dictionary<Type, RelationalTypeMapping> CreateClrMapMappings(IEnumerable<RelationalTypeMapping> typeMappings) =>
             new Dictionary<Type, RelationalTypeMapping> {
                    { typeof(int), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Integer)) },
                    { typeof(long), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Numeric)) },
                    { typeof(DateTime), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.DateTime)) },
                    { typeof(Guid), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Varchar)) },
                    { typeof(bool), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Logical)) },
                    { typeof(byte), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Integer)) },
                    { typeof(double), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Double)) },
                    { typeof(char), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Character)) },
                    { typeof(short), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Integer)) },
                    { typeof(float), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Float)) },
                    { typeof(decimal), typeMappings.Single(x => x.StoreType == GetVfpTypeString(VfpType.Numeric)) },
                    { typeof(string), new VfpStringTypeMapping(VfpType.Memo.ToVfpTypeName())},
                };

        private static Dictionary<string, RelationalTypeMapping> CreateStoreTypeMappings(IEnumerable<RelationalTypeMapping> typeMappings) =>
            typeMappings.ToDictionary(x => x.StoreType, x => x, StringComparer.OrdinalIgnoreCase);

        private static string GetVfpTypeString(VfpType vfpType) => vfpType.ToString().ToLower();
    }
}
