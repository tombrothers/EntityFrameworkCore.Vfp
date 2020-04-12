using System;

namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models {
    public class DiscontinuedProduct : Product {
        public DateTime? DiscontinuedDate { get; set; }
    }
}
