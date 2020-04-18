using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using VfpClient;

namespace EntityFrameworkCore.Vfp.Storage.Internal.TypeMappings {
    public class VfpStringTypeMapping : RelationalTypeMapping {
        private readonly VfpType? _vfpType;
        private readonly int _maxSpecificSize;

        public VfpStringTypeMapping(
            VfpType vfpType,
            int? size = null,
            bool fixedLength = false
        ) : this(vfpType.ToVfpTypeName(), size, fixedLength, vfpType) { }

        public VfpStringTypeMapping(
            [AllowNull] string storeType = null,
            int? size = null,
            bool fixedLength = false,
            VfpType? vfpType = null
        ) : this(new RelationalTypeMappingParameters(
                new CoreTypeMappingParameters(typeof(string)),
                storeType ?? GetStoreName(fixedLength, size),
                StoreTypePostfix.Size,
                GetDbType(fixedLength, size),
                false,
                size,
                fixedLength
            ),
            vfpType
        ) {
        }

        protected VfpStringTypeMapping(RelationalTypeMappingParameters parameters, VfpType? vfpType = null)
            : base(parameters) {
            _vfpType = vfpType;
            _maxSpecificSize = parameters.Size.HasValue && parameters.Size <= VfpMapping.MaximumCharacterFieldSize ? parameters.Size.Value : VfpMapping.MaximumCharacterFieldSize;
        }

        private static DbType? GetDbType(bool fixedLength, int? size) => GetVfpType(fixedLength, size).ToDbType();
        private static string GetStoreName(bool fixedLength, int? size) => GetVfpType(fixedLength, size).ToVfpTypeName();
        private static VfpType GetVfpType(bool fixedLength, int? size) => VfpMapping.GetVfpStringType(size ?? default, fixedLength);

        protected virtual string EscapeSqlLiteral([NotNull] string literal) {
            literal.ThrowIfNull(nameof(literal));

            return "'" + literal.Replace("'", "' + chr(39) + '") + "'";
        }

        protected override string GenerateNonNullSqlLiteral(object value) => EscapeSqlLiteral((string)value);
        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) => new VfpStringTypeMapping(parameters);
    }
}
