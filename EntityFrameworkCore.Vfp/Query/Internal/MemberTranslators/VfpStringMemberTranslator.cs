using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EntityFrameworkCore.Vfp.Query.Internal.MemberTranslators {
    public class VfpStringMemberTranslator : IMemberTranslator {
        private readonly ISqlExpressionFactory _sqlExpressionFactory;

        public VfpStringMemberTranslator([NotNull] ISqlExpressionFactory sqlExpressionFactory) {
            _sqlExpressionFactory = sqlExpressionFactory;
        }

        public virtual SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType) {
            member.ThrowIfNull(nameof(member));
            returnType.ThrowIfNull(nameof(returnType));

            if(member.Name == nameof(string.Length)
                && instance?.Type == typeof(string)) {
                return _sqlExpressionFactory.Convert(
                    _sqlExpressionFactory.Function(
                        "LEN",
                        new[] { instance },
                        true,
                        new[] { true },
                        typeof(int)),
                    returnType);
            }

            return null;
        }
    }
}
