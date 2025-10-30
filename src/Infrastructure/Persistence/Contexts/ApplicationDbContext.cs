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
    public DbSet<Location> Location { get; set; } = null!;
    public DbSet<InventoryEntry> InventoryEntry { get; set; } = null!;
    public DbSet<User> User { get; set; } = null!;
    public DbSet<Order> Order { get; set; } = null!;
    public DbSet<Cart> Cart { get; set; } = null!;
    public DbSet<OrderLine> OrderLine { get; set; } = null!;

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

      // Configure many-to-many relationship between Order and OrderLine
      modelBuilder.Entity<Order>()
        .HasMany(o => o.OrderLines)
        .WithMany()
        .UsingEntity<Dictionary<string, object>>(
          "OrderOrderLine",
          j => j
            .HasOne<OrderLine>()
            .WithMany()
            .HasForeignKey("OrderLineId"),
          j => j
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey("OrderId"),
          j =>
            j.HasKey("OrderId", "OrderLineId")
          );
        
      // Configure many-to-many relationship between Cart and OrderLine
      modelBuilder.Entity<Cart>()
        .HasMany(c => c.OrderLines)
        .WithMany()
        .UsingEntity<Dictionary<string, object>>(
          "CartOrderLine",
          j => j
            .HasOne<OrderLine>()
            .WithMany()
            .HasForeignKey("OrderLineId"),
          j => j
            .HasOne<Cart>()
            .WithMany()
            .HasForeignKey("CartId"),
          j =>
            j.HasKey("CartId", "OrderLineId")
          );
    }
  }
}