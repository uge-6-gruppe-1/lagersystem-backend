using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record UserUpdateDto
  {
    [Required]
    public Guid Id { get; init; }

    public string? Name { get; init; } = string.Empty;
  }
}