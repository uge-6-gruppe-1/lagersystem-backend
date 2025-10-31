using System.ComponentModel.DataAnnotations;
using Backend.Domain.ValueObjects;

namespace Backend.Application.DTOs
{
  public record UserCreateDto
  {
    [Required]
    public string Name { get; init; } = string.Empty;


    public string? Password { get; init; } = string.Empty; // Todo: Make non-nullable when implementing authentication
  }
}