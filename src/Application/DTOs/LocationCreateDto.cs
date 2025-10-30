using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record LocationCreateDto(
    [Required] string Name
  )
  { }
}