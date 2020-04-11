using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Vfp.Tests.Data.AutoGenId {
    public class AutoGenContext : DbContext {
        public DbSet<AutoGen> AutoGens { get; set; }

        private readonly string _connectionString;

        public AutoGenContext(string connectionString) {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseVfp(_connectionString);
    }
}
