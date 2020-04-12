using System;

namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models {
    public class Employee {
        public int EmployeeId { get; set; }
        public DateTime? HireDate { get; set; }
        public CommonAddress Address { get; set; }

        public Employee() {
            Address = new CommonAddress();
        }
    }
}
