using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record LocationUpdateDto(
    [Required] Guid Id,
    string? Name
  )
  { }
}