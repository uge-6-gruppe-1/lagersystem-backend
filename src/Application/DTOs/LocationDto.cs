using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record LocationDto(
    [Required] Guid Id,
    [Required] string Name
  )
  { }
}