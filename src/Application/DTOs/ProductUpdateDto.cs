using System.ComponentModel.DataAnnotations;
using Backend.Domain.ValueObjects;

namespace Backend.Application.DTOs
{
  public record ProductUpdateDto(
    [Required] Guid? Id,
    string? Name,
    string? Description,
    Price? Price,
    string? ImagePath
  )
  { }
}
