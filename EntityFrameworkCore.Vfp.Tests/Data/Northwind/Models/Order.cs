using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models {
    public class Order {
        public int OrderId { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipCountry { get; set; }
        public string ShipName { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? Freight { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public int EmployeeId { get; set; }

        public Order() {
            OrderDetails = new Collection<OrderDetail>();
        }
    }
}
