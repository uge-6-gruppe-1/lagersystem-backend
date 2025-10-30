using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Repositories
{
  public interface ILocationRepository
  {
    Task<LocationDto> Create(LocationCreateDto location);
    Task<IEnumerable<LocationDto>> GetAll();
    Task<LocationDto?> GetById(Guid id);
    Task<LocationDto?> Update(LocationUpdateDto location);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<InventoryEntryDto>> GetInventoryOfAllProductsAtLocation(Guid locationId);
    Task<InventoryEntryDto?> GetInventoryOfProductAtLocation(Guid locationId, Guid productId);
    Task<IEnumerable<InventoryEntryDto>> GetInventoryOfProductAtAllLocations(Guid locationId);
    Task<InventoryEntryDto?> UpdateInventoryOfProductAtLocation(Guid locationId, Guid productId, QuantityChangeDto quantityChange);
  }
}