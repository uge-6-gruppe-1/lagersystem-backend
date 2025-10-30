using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities
{
  public class Category
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public virtual ICollection<Product> Products { get; set; } = [];
  }
}