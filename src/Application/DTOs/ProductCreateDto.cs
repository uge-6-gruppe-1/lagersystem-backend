using System.ComponentModel.DataAnnotations;
using Backend.Domain.ValueObjects;

namespace Backend.Application.DTOs
{
  public record ProductCreateDto(
    [Required] string Name,
    [Required] string Description,
    [Required] Price Price,
    string? ImagePath
  )
  { }
}
