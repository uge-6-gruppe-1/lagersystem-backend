using Backend.Application.DTOs;
using Backend.Domain.Entities;

namespace Backend.Application.Mappers
{
  public static class InventoryEntryMapper
  {
    public static InventoryEntryDto ToDto(this InventoryEntry inventoryEntry)
    {
      return new InventoryEntryDto(
        inventoryEntry.ProductId,
        inventoryEntry.LocationId,
        inventoryEntry.Quantity
      );
    }
  }
}