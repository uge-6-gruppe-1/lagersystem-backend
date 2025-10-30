using System.ComponentModel.DataAnnotations;

namespace Backend.Application.DTOs
{
  public record InventoryEntryDto(
    [Required] Guid ProductId,
    [Required] Guid LocationId,
    [Required] int Quantity
  )
  { }
}