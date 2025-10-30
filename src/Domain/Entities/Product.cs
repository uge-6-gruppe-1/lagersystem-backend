using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities
{
  public class Product
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; } = decimal.Zero;

    public string ImagePath { get; set; } = string.Empty;

    public virtual ICollection<Category> Categories { get; set; } = [];
    
    public virtual ICollection<InventoryEntry> InventoryEntries { get; set; } = [];
  }
}