using APICarsGQL.Models;
using Microsoft.EntityFrameworkCore;

namespace APICarsGQL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CarsModel>()
                .HasOne(e => e.Owner)
                .WithMany(e => e.Cars)
                .HasForeignKey(e => e.OwnerId);

            modelBuilder
                .Entity<OwnersModel>()
                .HasMany(e => e.Cars)
                .WithOne(e => e.Owner)
                .HasForeignKey(e => e.OwnerId);

        }

        public DbSet<CarsModel> Cars { get; set; }
        public DbSet<OwnersModel> Owners { get; set; }
    }
}
