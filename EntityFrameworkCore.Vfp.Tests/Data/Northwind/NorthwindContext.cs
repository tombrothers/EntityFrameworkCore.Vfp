using EntityFrameworkCore.Vfp.Tests.Data.Northwind.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EntityFrameworkCore.Vfp.Tests.Data.Northwind {
    public class NorthwindContext : DbContext {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        private readonly string _connectionString;

        public NorthwindContext(string connectionString) {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseVfp(_connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
