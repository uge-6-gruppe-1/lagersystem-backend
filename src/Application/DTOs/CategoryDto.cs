using System.ComponentModel.DataAnnotations;
using Backend.Domain.ValueObjects;

namespace Backend.Application.DTOs
{
  public record CategoryDto(
    [Required] Guid Id,
    [Required] string Name,
    string? Description
    )
  { }
}