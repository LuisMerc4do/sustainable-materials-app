using Microsoft.EntityFrameworkCore;
using SustainableMaterialsRecommender.Core.Entities;

namespace SustainableMaterialsRecommender.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Material> Materials => Set<Material>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.SustainabilityScore).IsRequired();
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LegalRequirements).HasMaxLength(1000);
            entity.Property(e => e.ThreeDModelUrl).HasMaxLength(255);
        });
    }
}