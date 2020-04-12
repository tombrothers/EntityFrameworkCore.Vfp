namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models {
    public class OrderDetail {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
