using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models {
    public class Supplier {
        public int SupplierId { get; set; }
        public CommonAddress Address { get; set; }
        public ICollection<Product> Products { get; set; }

        public Supplier() {
            Address = new CommonAddress();
            Products = new Collection<Product>();
        }
    }
}
