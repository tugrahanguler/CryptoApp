using CryptoApp.DataModel;
using Microsoft.EntityFrameworkCore;

namespace CryptoApp.Entities
{
    public class CryptoContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName : "CryptoDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<CryptoDetail> UpdatedInformations { get; set; }
    }

}
