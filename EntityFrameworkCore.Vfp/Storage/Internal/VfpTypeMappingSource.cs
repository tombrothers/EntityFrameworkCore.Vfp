using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VfpClient;
using static System.String;

namespace EntityFrameworkCore.Vfp.Storage.Internal {
    public class VfpTypeMappingSource : RelationalTypeMappingSource {
        private readonly VfpTypeMapping _autoIncInteger = new VfpTypeMapping(VfpType.AutoIncInteger, typeof(int));
        private readonly VfpStringTypeMapping _binaryChar = new VfpStringTypeMapping(VfpType.BinaryCharacter, null, true);
        private readonly VfpStringTypeMapping _binaryMemo = new VfpStringTypeMapping(VfpType.BinaryMemo);
        private readonly VfpStringTypeMapping _binaryVarChar = new VfpStringTypeMapping(VfpType.BinaryVarchar);
        private readonly VfpTypeMapping _blob = new VfpTypeMapping(VfpType.Blob, typeof(byte[]));
        private readonly VfpStringTypeMapping _char = new VfpStringTypeMapping(VfpType.Character, null, true);
        private readonly VfpNumericTypeMapping _currency = new VfpNumericTypeMapping(VfpType.Currency);
        private readonly VfpTypeMapping _date = new VfpTypeMapping(VfpType.Date, typeof(DateTime));
        private readonly VfpTypeMapping _datetime = new VfpTypeMapping(VfpType.DateTime, typeof(DateTime));
        private readonly VfpTypeMapping _double = new VfpTypeMapping(VfpType.Double, typeof(double));
        private readonly VfpTypeMapping _float = new VfpTypeMapping(VfpType.Double, typeof(float));
        private readonly VfpTypeMapping _general = new VfpTypeMapping(VfpType.General, typeof(byte[]));
        private readonly VfpGuidTypeMapping _guid = new VfpGuidTypeMapping();
        private readonly VfpIntegerTypeMapping _interger = new VfpIntegerTypeMapping();
        private readonly VfpTypeMapping _logical = new VfpTypeMapping(VfpType.Logical, typeof(bool));
        private readonly VfpStringTypeMapping _memo = new VfpStringTypeMapping(VfpType.Memo);
        private readonly VfpNumericTypeMapping _numeric = new VfpNumericTypeMapping(VfpType.Numeric);
        private readonly VfpTypeMapping _varBinary = new VfpTypeMapping(VfpType.Varbinary, typeof(byte[]));
        private readonly VfpStringTypeMapping _varChar = new VfpStringTypeMapping(VfpType.Varchar);
        private readonly VfpTypeMapping _variant = new VfpTypeMapping(VfpType.Variant, typeof(object));

        private readonly Dictionary<Type, RelationalTypeMapping> _clrTypeMappings;
        private readonly Dictionary<string, RelationalTypeMapping> _storeTypeMappings;

        public VfpTypeMappingSource(
            [NotNull] TypeMappingSourceDependencies dependencies,
            [NotNull] RelationalTypeMappingSourceDependencies relationalDependencies
        ) : base(dependencies, relationalDependencies) {
            _clrTypeMappings = CreateClrMapMappings();
            _storeTypeMappings = CreateStoreTypeMappings();
        }

        protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo) {
            if(!IsNullOrWhiteSpace(mappingInfo.StoreTypeName)) {
                return _storeTypeMappings[mappingInfo.StoreTypeName];
            }

            if(mappingInfo.ClrType != null && _clrTypeMappings.TryGetValue(mappingInfo.ClrType, out var typeMapping)) {
                return typeMapping;
            }

            return null;
        }

        private Dictionary<Type, RelationalTypeMapping> CreateClrMapMappings() =>
             new Dictionary<Type, RelationalTypeMapping> {
                { typeof(bool), _logical },
                { typeof(DateTime), _datetime },
                { typeof(double), _double },
                { typeof(float), _float },
                { typeof(Guid), _guid },
                { typeof(long), new VfpTypeMapping(VfpType.Numeric, typeof(long)) },
                { typeof(short), _interger },
                { typeof(char), _char },
                { typeof(byte[]), _blob },
                { typeof(decimal), _numeric },
                { typeof(int), _interger },
                { typeof(string), _memo },



            };

        private Dictionary<string, RelationalTypeMapping> CreateStoreTypeMappings() =>
            new Dictionary<string, RelationalTypeMapping>(StringComparer.OrdinalIgnoreCase) {
                {  VfpStorageTypes.AutoIncInteger, _autoIncInteger },
                {  VfpStorageTypes.BinaryCharacter, _binaryChar },
                {  VfpStorageTypes.BinaryMemo, _binaryMemo },
                {  VfpStorageTypes.BinaryVarchar, _binaryVarChar },
                {  VfpStorageTypes.Blob, _blob },
                {  VfpStorageTypes.Character, _char },
                {  VfpStorageTypes.Currency, _currency },
                {  VfpStorageTypes.Date, _date },
                {  VfpStorageTypes.DateTime, _datetime },
                {  VfpStorageTypes.Double, _double },
                {  VfpStorageTypes.Float, _float },
                {  VfpStorageTypes.General, _general },
                {  VfpStorageTypes.Integer, _interger },
                {  VfpStorageTypes.Logical, _logical },
                {  VfpStorageTypes.Memo, _memo },
                {  VfpStorageTypes.Numeric, _numeric },
                {  VfpStorageTypes.Varbinary, _varBinary },
                {  VfpStorageTypes.Varchar, _varChar },
                {  VfpStorageTypes.Variant, _variant },
            };
    }
}