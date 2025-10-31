using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record UserDto
  {
    [Required]
    public Guid Id { get; init; }

    [Required]
    public string Name { get; init; } = string.Empty;
  }
}