using Microsoft.EntityFrameworkCore;
using Backend.Domain.Entities;

namespace Backend.Infrastructure.Persistence.Contexts
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Add DbSets for entities
    public DbSet<Product> Product { get; set; } = null!;
    public DbSet<Category> Category { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure many-to-many relationship between Product and Category
      modelBuilder.Entity<Product>()
        .HasMany(p => p.Categories)
        .WithMany(c => c.Products)
        .UsingEntity<Dictionary<string, object>>(
          "ProductCategory",
          j => j
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey("CategoryId"),
          j => j
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey("ProductId"),
          j =>
            j.HasKey("ProductId", "CategoryId")
          );
    }
  }
}