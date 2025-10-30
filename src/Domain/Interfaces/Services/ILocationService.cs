using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Services
{
  public interface ILocationService
  {
    Task<LocationDto> Create(LocationCreateDto locationCreateDto);
    Task<IEnumerable<LocationDto>> GetAll();
    Task<LocationDto?> GetById(Guid id);
    Task<LocationDto?> Update(LocationUpdateDto locationUpdateDto);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<InventoryEntryDto>> GetInventoryOfAllProductsAtLocation(Guid locationId);
    Task<InventoryEntryDto?> GetInventoryOfProductAtLocation(Guid locationId, Guid productId);
    Task<IEnumerable<InventoryEntryDto>> GetInventoryOfProductAtAllLocations(Guid locationId);
    Task<InventoryEntryDto?> UpdateInventoryOfProductAtLocation(Guid locationId, Guid productId, QuantityChangeDto quantityChange);
  }
}