using Microsoft.EntityFrameworkCore.ValueGeneration;
using EntityFrameworkCore.Vfp.ValueGeneration.Internal;

namespace EntityFrameworkCore.Vfp.ValueGeneration {
    public class VfpValueGeneratorCache : ValueGeneratorCache, IVfpValueGeneratorCache {
        public VfpValueGeneratorCache(ValueGeneratorCacheDependencies dependencies) : base(dependencies) {
        }
    }
}
