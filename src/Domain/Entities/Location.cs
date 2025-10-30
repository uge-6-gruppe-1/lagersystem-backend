using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities
{
  public class Location
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<InventoryEntry> InventoryEntries { get; set; } = new List<InventoryEntry>();
  }
}