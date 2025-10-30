using System.ComponentModel.DataAnnotations;
using Backend.Domain.ValueObjects;

namespace Backend.Application.DTOs
{
  public record ProductDto(
    [Required] Guid Id,
    [Required] string Name,
    [Required] string Description,
    [Required] Price Price,
    string? ImagePath,
    IEnumerable<InventoryEntryDto>? Inventory
  )
  { }
}
