using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Vfp.Tests.Data.AllTypes {
    public class AllTypesContext : DbContext {
        public DbSet<AllTypesTable> AllTypes { get; set; }

        private readonly string _connectionString;

        public AllTypesContext(string connectionString) {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseVfp(_connectionString);
    }
}
