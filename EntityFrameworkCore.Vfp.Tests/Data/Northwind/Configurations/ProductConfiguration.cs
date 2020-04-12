using EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind.Configurations {
    public class ProductConfiguration : IEntityTypeConfiguration<Product> {
        public void Configure(EntityTypeBuilder<Product> builder) {
            builder.HasKey(x => x.ProductId);
            builder.ToTable("Products");
            builder.HasDiscriminator<bool>("Discontinued").HasValue<Product>(false).HasValue<DiscontinuedProduct>(true);
        }
    }
}
