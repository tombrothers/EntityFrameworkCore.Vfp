using System.Collections.Generic;

namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models {
    public class Product {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? ReorderLevel { get; set; }
        public int? UnitsInStock { get; set; }
        public int? UnitsOnOrder { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
