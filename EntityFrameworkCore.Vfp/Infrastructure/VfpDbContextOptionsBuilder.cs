using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using EntityFrameworkCore.Vfp.Infrastructure.Internal;

namespace EntityFrameworkCore.Vfp.Infrastructure {
    public class VfpDbContextOptionsBuilder
            : RelationalDbContextOptionsBuilder<VfpDbContextOptionsBuilder, VfpOptionsExtension> {

        public VfpDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
            : base(optionsBuilder) {
        }
    }
}
