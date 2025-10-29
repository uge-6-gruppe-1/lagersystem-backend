using System.ComponentModel.DataAnnotations;
using Backend.Domain.ValueObjects;

namespace Backend.Application.DTOs
{
  public record CategoryUpdateDto(
    [Required] Guid Id,
    string? Name,
    string? Description
    )
  { }
}