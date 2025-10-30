using System.ComponentModel.DataAnnotations;

namespace Backend.Domain.Entities
{
  public class User
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;
  }
}