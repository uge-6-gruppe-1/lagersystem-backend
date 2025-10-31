using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record CartCreateDto
  {
    public Guid? UserId { get; init; }

    public string? Name { get; init; } = string.Empty;
  }
}