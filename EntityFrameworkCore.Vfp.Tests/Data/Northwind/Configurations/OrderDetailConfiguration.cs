using EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind.Configurations {
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail> {
        public void Configure(EntityTypeBuilder<OrderDetail> builder) {
            builder.HasKey(x => new { x.OrderId, x.ProductId });
        }
    }
}
