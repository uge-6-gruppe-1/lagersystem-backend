using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record OrderUpdateDto
  {
    [Required]
    public Guid Id { get; init; }

    public Guid? UserId { get; init; }

    public string? Name { get; init; } = string.Empty;
  }
}