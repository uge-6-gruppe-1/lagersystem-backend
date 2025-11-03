using Moq;
using Xunit;
using FluentAssertions;
using Backend.Application.Services;
using Backend.Domain.Interfaces.Repositories;
using Backend.Application.DTOs;
using Backend.Domain.Enums;

namespace Backend.Tests.Application.Services
{
  public class LocationServiceTests
  {
    private readonly Mock<ILocationRepository> _mockLocationRepository;
    private readonly LocationService _locationService;

    public LocationServiceTests()
    {
      _mockLocationRepository = new Mock<ILocationRepository>();
      _locationService = new LocationService(_mockLocationRepository.Object);
    }

    #region Create Tests

    [Fact]
    public async Task Create_ValidLocationCreateDto_ReturnsCreatedLocationDto()
    {
      // Arrange
      var locationCreateDto = new LocationCreateDto("Test Warehouse");
      var expectedLocationDto = new LocationDto(Guid.NewGuid(), "Test Warehouse");

      _mockLocationRepository
        .Setup(repo => repo.Create(locationCreateDto))
        .ReturnsAsync(expectedLocationDto);

      // Act
      var result = await _locationService.Create(locationCreateDto);

      // Assert
      result.Should().BeEquivalentTo(expectedLocationDto);
      _mockLocationRepository.Verify(repo => repo.Create(locationCreateDto), Times.Once);
    }

    #endregion

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenLocationsExist_ReturnsAllLocations()
    {
      // Arrange
      var locations = new List<LocationDto>
      {
        new LocationDto(Guid.NewGuid(), "Warehouse A"),
        new LocationDto(Guid.NewGuid(), "Warehouse B"),
        new LocationDto(Guid.NewGuid(), "Store Front")
      };

      _mockLocationRepository
        .Setup(repo => repo.GetAll())
        .ReturnsAsync(locations);

      // Act
      var result = await _locationService.GetAll();

      // Assert
      result.Should().BeEquivalentTo(locations);
      result.Should().HaveCount(3);
      _mockLocationRepository.Verify(repo => repo.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenNoLocationsExist_ReturnsEmptyCollection()
    {
      // Arrange
      _mockLocationRepository
        .Setup(repo => repo.GetAll())
        .ReturnsAsync(new List<LocationDto>());

      // Act
      var result = await _locationService.GetAll();

      // Assert
      result.Should().BeEmpty();
      _mockLocationRepository.Verify(repo => repo.GetAll(), Times.Once);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_ExistingId_ReturnsLocationDto()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var expectedLocation = new LocationDto(locationId, "Test Warehouse");

      _mockLocationRepository
        .Setup(repo => repo.GetById(locationId))
        .ReturnsAsync(expectedLocation);

      // Act
      var result = await _locationService.GetById(locationId);

      // Assert
      result.Should().BeEquivalentTo(expectedLocation);
      _mockLocationRepository.Verify(repo => repo.GetById(locationId), Times.Once);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNull()
    {
      // Arrange
      var locationId = Guid.NewGuid();

      _mockLocationRepository
        .Setup(repo => repo.GetById(locationId))
        .ReturnsAsync((LocationDto?)null);

      // Act
      var result = await _locationService.GetById(locationId);

      // Assert
      result.Should().BeNull();
      _mockLocationRepository.Verify(repo => repo.GetById(locationId), Times.Once);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_ValidLocationUpdateDto_ReturnsUpdatedLocationDto()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var locationUpdateDto = new LocationUpdateDto(locationId, "Updated Warehouse");
      var expectedUpdatedLocation = new LocationDto(locationId, "Updated Warehouse");

      _mockLocationRepository
        .Setup(repo => repo.Update(locationUpdateDto))
        .ReturnsAsync(expectedUpdatedLocation);

      // Act
      var result = await _locationService.Update(locationUpdateDto);

      // Assert
      result.Should().BeEquivalentTo(expectedUpdatedLocation);
      _mockLocationRepository.Verify(repo => repo.Update(locationUpdateDto), Times.Once);
    }

    [Fact]
    public async Task Update_NonExistingLocation_ReturnsNull()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var locationUpdateDto = new LocationUpdateDto(locationId, "Updated Warehouse");

      _mockLocationRepository
        .Setup(repo => repo.Update(locationUpdateDto))
        .ReturnsAsync((LocationDto?)null);

      // Act
      var result = await _locationService.Update(locationUpdateDto);

      // Assert
      result.Should().BeNull();
      _mockLocationRepository.Verify(repo => repo.Update(locationUpdateDto), Times.Once);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_ExistingId_ReturnsTrue()
    {
      // Arrange
      var locationId = Guid.NewGuid();

      _mockLocationRepository
        .Setup(repo => repo.Delete(locationId))
        .ReturnsAsync(true);

      // Act
      var result = await _locationService.Delete(locationId);

      // Assert
      result.Should().BeTrue();
      _mockLocationRepository.Verify(repo => repo.Delete(locationId), Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsFalse()
    {
      // Arrange
      var locationId = Guid.NewGuid();

      _mockLocationRepository
        .Setup(repo => repo.Delete(locationId))
        .ReturnsAsync(false);

      // Act
      var result = await _locationService.Delete(locationId);

      // Assert
      result.Should().BeFalse();
      _mockLocationRepository.Verify(repo => repo.Delete(locationId), Times.Once);
    }

    #endregion

    #region GetInventoryOfProductAtLocation Tests

    [Fact]
    public async Task GetInventoryOfProductAtLocation_ExistingInventory_ReturnsInventoryEntry()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var productId = Guid.NewGuid();
      var expectedInventoryEntry = new InventoryEntryDto(productId, locationId, 50);

      _mockLocationRepository
        .Setup(repo => repo.GetInventoryOfProductAtLocation(locationId, productId))
        .ReturnsAsync(expectedInventoryEntry);

      // Act
      var result = await _locationService.GetInventoryOfProductAtLocation(locationId, productId);

      // Assert
      result.Should().BeEquivalentTo(expectedInventoryEntry);
      _mockLocationRepository.Verify(repo => repo.GetInventoryOfProductAtLocation(locationId, productId), Times.Once);
    }

    [Fact]
    public async Task GetInventoryOfProductAtLocation_NonExistingInventory_ReturnsNull()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var productId = Guid.NewGuid();

      _mockLocationRepository
        .Setup(repo => repo.GetInventoryOfProductAtLocation(locationId, productId))
        .ReturnsAsync((InventoryEntryDto?)null);

      // Act
      var result = await _locationService.GetInventoryOfProductAtLocation(locationId, productId);

      // Assert
      result.Should().BeNull();
      _mockLocationRepository.Verify(repo => repo.GetInventoryOfProductAtLocation(locationId, productId), Times.Once);
    }

    #endregion

    #region GetInventoryOfAllProductsAtLocation Tests

    [Fact]
    public async Task GetInventoryOfAllProductsAtLocation_ExistingInventory_ReturnsInventoryEntries()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var inventoryEntries = new List<InventoryEntryDto>
      {
        new InventoryEntryDto(Guid.NewGuid(), locationId, 25),
        new InventoryEntryDto(Guid.NewGuid(), locationId, 50),
        new InventoryEntryDto(Guid.NewGuid(), locationId, 0)
      };

      _mockLocationRepository
        .Setup(repo => repo.GetInventoryOfAllProductsAtLocation(locationId))
        .ReturnsAsync(inventoryEntries);

      // Act
      var result = await _locationService.GetInventoryOfAllProductsAtLocation(locationId);

      // Assert
      result.Should().BeEquivalentTo(inventoryEntries);
      result.Should().HaveCount(3);
      _mockLocationRepository.Verify(repo => repo.GetInventoryOfAllProductsAtLocation(locationId), Times.Once);
    }

    [Fact]
    public async Task GetInventoryOfAllProductsAtLocation_NoInventory_ReturnsEmptyCollection()
    {
      // Arrange
      var locationId = Guid.NewGuid();

      _mockLocationRepository
        .Setup(repo => repo.GetInventoryOfAllProductsAtLocation(locationId))
        .ReturnsAsync(new List<InventoryEntryDto>());

      // Act
      var result = await _locationService.GetInventoryOfAllProductsAtLocation(locationId);

      // Assert
      result.Should().BeEmpty();
      _mockLocationRepository.Verify(repo => repo.GetInventoryOfAllProductsAtLocation(locationId), Times.Once);
    }

    #endregion

    #region GetInventoryOfProductAtAllLocations Tests

    [Fact]
    public async Task GetInventoryOfProductAtAllLocations_ExistingInventory_ReturnsInventoryEntries()
    {
      // Arrange
      var productId = Guid.NewGuid();
      var inventoryEntries = new List<InventoryEntryDto>
      {
        new InventoryEntryDto(productId, Guid.NewGuid(), 25),
        new InventoryEntryDto(productId, Guid.NewGuid(), 50),
        new InventoryEntryDto(productId, Guid.NewGuid(), 0)
      };

      _mockLocationRepository
        .Setup(repo => repo.GetInventoryOfProductAtAllLocations(productId))
        .ReturnsAsync(inventoryEntries);

      // Act
      var result = await _locationService.GetInventoryOfProductAtAllLocations(productId);

      // Assert
      result.Should().BeEquivalentTo(inventoryEntries);
      result.Should().HaveCount(3);
      _mockLocationRepository.Verify(repo => repo.GetInventoryOfProductAtAllLocations(productId), Times.Once);
    }

    #endregion

    #region UpdateInventoryOfProductAtLocation Tests

    [Fact]
    public async Task UpdateInventoryOfProductAtLocation_ValidIncrement_ReturnsUpdatedInventoryEntry()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var productId = Guid.NewGuid();
      var quantityChange = new QuantityChangeDto(AdjustmentType.INCREMENT, 10);
      var expectedInventoryEntry = new InventoryEntryDto(productId, locationId, 60);

      _mockLocationRepository
        .Setup(repo => repo.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange))
        .ReturnsAsync(expectedInventoryEntry);

      // Act
      var result = await _locationService.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange);

      // Assert
      result.Should().BeEquivalentTo(expectedInventoryEntry);
      _mockLocationRepository.Verify(repo => repo.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange), Times.Once);
    }

    [Fact]
    public async Task UpdateInventoryOfProductAtLocation_ValidDecrement_ReturnsUpdatedInventoryEntry()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var productId = Guid.NewGuid();
      var quantityChange = new QuantityChangeDto(AdjustmentType.DECREMENT, 5);
      var expectedInventoryEntry = new InventoryEntryDto(productId, locationId, 45);

      _mockLocationRepository
        .Setup(repo => repo.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange))
        .ReturnsAsync(expectedInventoryEntry);

      // Act
      var result = await _locationService.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange);

      // Assert
      result.Should().BeEquivalentTo(expectedInventoryEntry);
      _mockLocationRepository.Verify(repo => repo.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange), Times.Once);
    }

    [Fact]
    public async Task UpdateInventoryOfProductAtLocation_ValidSet_ReturnsUpdatedInventoryEntry()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var productId = Guid.NewGuid();
      var quantityChange = new QuantityChangeDto(AdjustmentType.SET, 100);
      var expectedInventoryEntry = new InventoryEntryDto(productId, locationId, 100);

      _mockLocationRepository
        .Setup(repo => repo.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange))
        .ReturnsAsync(expectedInventoryEntry);

      // Act
      var result = await _locationService.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange);

      // Assert
      result.Should().BeEquivalentTo(expectedInventoryEntry);
      _mockLocationRepository.Verify(repo => repo.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange), Times.Once);
    }

    [Fact]
    public async Task UpdateInventoryOfProductAtLocation_InsufficientInventoryForDecrement_ReturnsNull()
    {
      // Arrange
      var locationId = Guid.NewGuid();
      var productId = Guid.NewGuid();
      var quantityChange = new QuantityChangeDto(AdjustmentType.DECREMENT, 100);

      _mockLocationRepository
        .Setup(repo => repo.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange))
        .ReturnsAsync((InventoryEntryDto?)null);

      // Act
      var result = await _locationService.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange);

      // Assert
      result.Should().BeNull();
      _mockLocationRepository.Verify(repo => repo.UpdateInventoryOfProductAtLocation(locationId, productId, quantityChange), Times.Once);
    }

    #endregion
  }
}