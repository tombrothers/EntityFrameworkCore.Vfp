using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models;

namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind {
    public class NorthwindContext : DbContext {
        public DbSet<Customer> Customers { get; set; }

        private readonly string _connectionString;

        public NorthwindContext(string connectionString) {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseVfp(_connectionString);
    }
}
