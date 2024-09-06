using Microsoft.EntityFrameworkCore;
using SustainableMaterialsRecommender.Domain.Entities;

namespace SustainableMaterialsRecommender.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Material>().ToTable("Materials");
            // Add any additional configuration for the Material entity if needed
        }
    }
}