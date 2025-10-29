using System.ComponentModel.DataAnnotations;
using Backend.Domain.ValueObjects;

namespace Backend.Application.DTOs
{
  public record CategoryCreateDto(
    [Required] string Name,
    string? Description
    )
  { }
}