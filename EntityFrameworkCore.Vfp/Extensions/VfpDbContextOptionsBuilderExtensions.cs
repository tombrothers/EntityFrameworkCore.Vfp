using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using EntityFrameworkCore.Vfp;
using EntityFrameworkCore.Vfp.Infrastructure;
using EntityFrameworkCore.Vfp.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore {
    public static class VfpDbContextOptionsExtensions {
        public static DbContextOptionsBuilder<TContext> UseVfp<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
            [AllowNull] Action<VfpDbContextOptionsBuilder> vfpOptionsAction = null
        ) where TContext : DbContext =>
            (DbContextOptionsBuilder<TContext>)UseVfp((DbContextOptionsBuilder)optionsBuilder, vfpOptionsAction);

        public static DbContextOptionsBuilder UseVfp(
            [NotNull] this DbContextOptionsBuilder optionsBuilder,
            [AllowNull] Action<VfpDbContextOptionsBuilder> vfpOptionsAction = null
        ) {
            optionsBuilder.ThrowIfNull(nameof(optionsBuilder));

            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(GetOrCreateExtension(optionsBuilder));

            vfpOptionsAction?.Invoke(new VfpDbContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }

        public static DbContextOptionsBuilder<TContext> UseVfp<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
            [NotNull] string connectionString,
            [AllowNull] Action<VfpDbContextOptionsBuilder> vfpOptionsAction = null
        ) where TContext : DbContext =>
            (DbContextOptionsBuilder<TContext>)UseVfp((DbContextOptionsBuilder)optionsBuilder, connectionString, vfpOptionsAction);

        public static DbContextOptionsBuilder UseVfp(
            [NotNull] this DbContextOptionsBuilder optionsBuilder,
            [NotNull] string connectionString,
            [AllowNull] Action<VfpDbContextOptionsBuilder> vfpOptionsAction = null
        ) {
            optionsBuilder.ThrowIfNull(nameof(optionsBuilder));
            connectionString.ThrowIfNull(nameof(connectionString));

            var extension = (VfpOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            vfpOptionsAction?.Invoke(new VfpDbContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }

        public static DbContextOptionsBuilder<TContext> UseVfp<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
            [NotNull] DbConnection connection,
            [AllowNull] Action<VfpDbContextOptionsBuilder> vfpOptionsAction = null
        ) where TContext : DbContext =>
            (DbContextOptionsBuilder<TContext>)UseVfp((DbContextOptionsBuilder)optionsBuilder, connection, vfpOptionsAction);

        public static DbContextOptionsBuilder UseVfp(
            [NotNull] this DbContextOptionsBuilder optionsBuilder,
            [NotNull] DbConnection connection,
            [AllowNull] Action<VfpDbContextOptionsBuilder> vfpOptionsAction = null
        ) {
            optionsBuilder.ThrowIfNull(nameof(optionsBuilder));
            connection.ThrowIfNull(nameof(connection));

            var extension = (VfpOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnection(connection);
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            vfpOptionsAction?.Invoke(new VfpDbContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }

        private static VfpOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.Options.FindExtension<VfpOptionsExtension>() ?? new VfpOptionsExtension();
    }
}
