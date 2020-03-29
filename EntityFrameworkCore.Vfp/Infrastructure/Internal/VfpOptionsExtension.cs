using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using EntityFrameworkCore.Vfp.Extensions;

namespace EntityFrameworkCore.Vfp.Infrastructure.Internal {
    public class VfpOptionsExtension : RelationalOptionsExtension {
        private DbContextOptionsExtensionInfo _info;

        public VfpOptionsExtension() {
        }

        protected VfpOptionsExtension(VfpOptionsExtension copyFrom)
            : base(copyFrom) {
        }

        public override DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);
        protected override RelationalOptionsExtension Clone() => new VfpOptionsExtension(this);

        public override void ApplyServices([NotNull] IServiceCollection services) => services.AddEntityFrameworkVfp();

        private sealed class ExtensionInfo : RelationalExtensionInfo {
            private string _logFragment;

            public ExtensionInfo(IDbContextOptionsExtension extension)
                : base(extension) {
            }

            private new VfpOptionsExtension Extension => (VfpOptionsExtension)base.Extension;

            public override bool IsDatabaseProvider => true;

            public override string LogFragment {
                get {
                    if(_logFragment == null) {
                        var builder = new StringBuilder();

                        builder.Append(base.LogFragment);

                        _logFragment = builder.ToString();
                    }

                    return _logFragment;
                }
            }

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) => debugInfo["Vfp"] = "1";
        }
    }
}
