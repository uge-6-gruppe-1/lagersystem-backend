using Microsoft.EntityFrameworkCore;
using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Application.Mappers;
using Backend.Application.DTOs;
using Backend.Infrastructure.Persistence.Contexts;

namespace Backend.Infrastructure.Persistence.Repositories
{
  public class LocationRepository(ApplicationDbContext context) : ILocationRepository
  {
    private readonly ApplicationDbContext _context = context;

    public async Task<LocationDto> Create(LocationCreateDto locationCreateDto)
    {
      var locationDto = locationCreateDto.ToDto();
      _context.Location.Add(locationDto.ToEntity());
      await _context.SaveChangesAsync();
      return locationDto;
    }

    public async Task<bool> Delete(Guid id)
    {
      var location = await _context.Location.FindAsync(id);
      if (location == null) return false;
      _context.Location.Remove(location);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<IEnumerable<LocationDto>> GetAll()
    {
      return await _context.Location.Select(location => location.ToDto()).ToListAsync();
    }

    public async Task<LocationDto?> GetById(Guid id)
    {
      var location = await _context.Location.FindAsync(id);
      return location?.ToDto();
    }

    public async Task<LocationDto?> Update(LocationUpdateDto location)
    {
      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        // Get existing location
        var existingLocation = await _context.Location
          .Where(l => l.Id == location.Id)
          .FirstOrDefaultAsync();

        if (existingLocation == null)
        {
          await transaction.RollbackAsync();
          return null;
        }

        // Apply updates
        existingLocation = location.ApplyUpdatesToEntity(existingLocation);

        // Save changes
        _context.Location.Update(existingLocation);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return existingLocation.ToDto();
      }
      catch
      {
        // Rollback transaction on error
        await transaction.RollbackAsync();
        throw;
      }
    }

    public async Task<IEnumerable<InventoryEntryDto>> GetInventoryOfAllProductsAtLocation(Guid locationId)
    {
      return await _context.InventoryEntry
        .Where(entry => entry.LocationId == locationId)
        .Select(entry => entry.ToDto())
        .ToListAsync();
    }

    public async Task<InventoryEntryDto?> GetInventoryOfProductAtLocation(Guid locationId, Guid productId)
    {
      var inventoryEntry = await _context.InventoryEntry
        .Where(entry => entry.LocationId == locationId && entry.ProductId == productId)
        .FirstOrDefaultAsync();
      return inventoryEntry?.ToDto();
    }

    public async Task<IEnumerable<InventoryEntryDto>> GetInventoryOfProductAtAllLocations(Guid locationId)
    {
      return await _context.InventoryEntry
        .Where(entry => entry.LocationId == locationId)
        .Select(entry => entry.ToDto())
        .ToListAsync();
    }

    public async Task<InventoryEntryDto?> UpdateInventoryOfProductAtLocation(Guid locationId, Guid productId, QuantityChangeDto quantityChange)
    {
      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        // Get existing inventory entry
        var existingEntry = await _context.InventoryEntry
          .Where(entry => entry.LocationId == locationId && entry.ProductId == productId)
          .FirstOrDefaultAsync();

        if (existingEntry == null)
        {
          // Entry does not exist, create new entry with initial quantity 0
          existingEntry = new InventoryEntry
          {
            LocationId = locationId,
            ProductId = productId,
            Quantity = 0
          };
          _context.InventoryEntry.Add(existingEntry);
        }

        // Apply quantity change
        if (quantityChange.operation == AdjustmentType.INCREMENT)
        {
          existingEntry.Quantity += quantityChange.amount;
        }
        else if (quantityChange.operation == AdjustmentType.DECREMENT)
        {
          if (existingEntry.Quantity < quantityChange.amount)
          {
            // Decrement would lead to negative inventory, rollback transaction and return null
            await transaction.RollbackAsync();
            return null;
          }
          existingEntry.Quantity -= quantityChange.amount;
        }
        else if (quantityChange.operation == AdjustmentType.SET)
        {
          existingEntry.Quantity = quantityChange.amount;
        }

        // Save changes
        _context.InventoryEntry.Update(existingEntry);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return existingEntry.ToDto();
      }
      catch
      {
        // Rollback transaction on error
        await transaction.RollbackAsync();
        throw;
      }
    }
  }
}