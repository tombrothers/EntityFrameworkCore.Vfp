using EntityFrameworkCore.Vfp.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace EntityFrameworkCore.Vfp.Update.Internal {
    public class VfpModificationCommandBatchFactory : IModificationCommandBatchFactory {
        private readonly ModificationCommandBatchFactoryDependencies _dependencies;
        private readonly IDbContextOptions _options;

        public VfpModificationCommandBatchFactory(
            [NotNull] ModificationCommandBatchFactoryDependencies dependencies,
            [NotNull] IDbContextOptions options) {
            dependencies.ThrowIfNull(nameof(dependencies));
            options.ThrowIfNull(nameof(options));

            _dependencies = dependencies;
            _options = options;
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual ModificationCommandBatch Create() {
            var optionsExtension = _options.Extensions.OfType<VfpOptionsExtension>().FirstOrDefault();

            return new VfpModificationCommandBatch(_dependencies, optionsExtension?.MaxBatchSize);
        }
    }
}
