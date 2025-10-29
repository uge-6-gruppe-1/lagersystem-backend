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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }
  }
}