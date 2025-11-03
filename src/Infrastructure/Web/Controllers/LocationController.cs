using Microsoft.AspNetCore.Mvc;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;

namespace Backend.Infrastructure.Web.Controllers
{
  [ApiController]
  [Route("api/locations")]
  public class LocationController : ControllerBase
  {
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
      _locationService = locationService;
    }

    // GET: api/locations - get all locations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
    {
      var locationDtos = await _locationService.GetAll();
      // Return 200 with locations
      return Ok(locationDtos);
    }

    // GET: api/locations/{locationId} - get location by ID
    [HttpGet("{locationId}")]
    public async Task<ActionResult<LocationDto>> GetLocation(Guid locationId)
    {
      var locationDto = await _locationService.GetById(locationId);
      // Return 404 if not found
      if (locationDto == null) return NotFound($"Location with ID {locationId} not found.");
      // Return 200 with location
      return Ok(locationDto);
    }

    // POST: api/locations - create new location
    [HttpPost]
    public async Task<ActionResult<LocationDto>> CreateLocation([FromBody] LocationCreateDto locationCreateDto)
    {
      // Return 400 if request validation fails
      if (!ModelState.IsValid) return BadRequest(ModelState);
      var createdLocation = await _locationService.Create(locationCreateDto);
      // Return 201 with location header
      return CreatedAtAction(nameof(GetLocation), new { id = createdLocation.Id }, createdLocation);
    }

    // PATCH: api/locations/ - update location
    [HttpPatch]
    public async Task<ActionResult<LocationDto>> UpdateLocation([FromBody] LocationUpdateDto locationUpdateDto)
    {
      // Return 400 if request validation fails
      if (!ModelState.IsValid) return BadRequest(ModelState);
      var updatedLocation = await _locationService.Update(locationUpdateDto);
      // Return 404 if not found
      if (updatedLocation == null) return NotFound($"Location with ID {locationUpdateDto.Id} not found.");
      // Return 200 with updated location
      return Ok(updatedLocation);
    }

    // DELETE: api/locations/{locationId} - delete location by ID
    [HttpDelete("{locationId}")]
    public async Task<IActionResult> DeleteLocation(Guid locationId)
    {
      var deleted = await _locationService.Delete(locationId);
      // Return 404 if not found
      if (!deleted) return NotFound($"Location with ID {locationId} not found.");
      // Return 204 No Content on successful deletion
      return NoContent();
    }

    // GET: api/locations/{locationId}/products - get inventory entries for all products in a location
    [HttpGet("{locationId}/products")]
    public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetProductsAtLocation(Guid locationId)
    {
      var inventoryEntries = await _locationService.GetInventoryOfAllProductsAtLocation(locationId);
      // Return 200 with inventory entries
      return Ok(inventoryEntries);
    }

    // GET: api/locations/{id}/products/{productId} - get inventory entry for a specific product in a location
    [HttpGet("{locationId}/products/{productId}")]
    public async Task<ActionResult<InventoryEntryDto>> GetProductAtLocation(Guid locationId, Guid productId)
    {
      var inventoryEntry = await _locationService.GetInventoryOfProductAtLocation(locationId, productId);
      // Return 404 if not found
      if (inventoryEntry == null) return NotFound($"Product with ID {productId} not found in location {locationId}.");
      // Return 200 with inventory entry
      return Ok(inventoryEntry);
    }

    // PATCH: api/locations/{id}/products/{productId} - Change inventory for a specific product in a location
    [HttpPatch("{locationId}/products/{productId}")]
    public async Task<ActionResult<InventoryEntryDto>> UpdateProductInventory(
      Guid locationId,
      Guid productId,
      [FromBody] QuantityChangeDto quantityChangeDto)
    {
      // Return 400 if request validation fails
      if (!ModelState.IsValid) return BadRequest(ModelState);
      var updatedInventoryEntry = await _locationService.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChangeDto);
      // Return 404 if not found
      if (updatedInventoryEntry == null) return NotFound($"Product with ID {productId} not found in location {locationId}.");
      // Return 200 with updated inventory entry
      return Ok(updatedInventoryEntry);
    }
  }
}