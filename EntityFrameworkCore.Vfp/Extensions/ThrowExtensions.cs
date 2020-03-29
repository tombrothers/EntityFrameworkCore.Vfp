using System;
using System.Diagnostics.CodeAnalysis;
using static System.String;

namespace EntityFrameworkCore.Vfp {
    internal static class ThrowExtensions {
        public static string ThrowIfNullOrEmpty([AllowNull] this string value, [NotNull] string argumentName) {
            value.ThrowIfNull(argumentName);

            if(IsNullOrWhiteSpace(value)) {
                throw new ArgumentException(argumentName);
            }

            return value;
        }

        public static T ThrowIfNull<T>([AllowNull] this T value, [NotNull] string argumentName) where T : class {
            if(value == null) {
                throw new ArgumentNullException(argumentName);
            }

            return value;
        }
    }
}
