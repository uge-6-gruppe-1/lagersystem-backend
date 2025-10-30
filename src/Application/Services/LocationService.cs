using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;

namespace Backend.Application.Services
{
  public class LocationService : ILocationService
  {
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
      _locationRepository = locationRepository;
    }

    public async Task<LocationDto> Create(LocationCreateDto locationCreateDto)
    {
      var createdLocationDto = await _locationRepository.Create(locationCreateDto);
      return createdLocationDto;
    }

    public async Task<IEnumerable<LocationDto>> GetAll()
    {
      return await _locationRepository.GetAll();
    }

    public async Task<LocationDto?> GetById(Guid id)
    {
      return await _locationRepository.GetById(id);
    }

    public async Task<LocationDto?> Update(LocationUpdateDto locationUpdateDto)
    {
      return await _locationRepository.Update(locationUpdateDto);
    }

    public async Task<bool> Delete(Guid id)
    {
      return await _locationRepository.Delete(id);
    }

    public async Task<InventoryEntryDto?> GetInventoryOfProductAtLocation(Guid locationId, Guid productId)
    {
      return await _locationRepository.GetInventoryOfProductAtLocation(locationId, productId);
    }

    public async Task<IEnumerable<InventoryEntryDto>> GetInventoryOfAllProductsAtLocation(Guid locationId)
    {
      return await _locationRepository.GetInventoryOfAllProductsAtLocation(locationId);
    }

    public async Task<IEnumerable<InventoryEntryDto>> GetInventoryOfProductAtAllLocations(Guid locationId)
    {
      return await _locationRepository.GetInventoryOfProductAtAllLocations(locationId);
    }

    public async Task<InventoryEntryDto?> UpdateInventoryOfProductAtLocation(Guid locationId, Guid productId, QuantityChangeDto quantityChange)
    {
      return await _locationRepository.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange);
    }
  }
}