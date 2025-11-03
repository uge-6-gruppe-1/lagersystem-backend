using Moq;
using Xunit;
using FluentAssertions;
using Backend.Application.Services;
using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;
using Backend.Domain.ValueObjects;

namespace Backend.Tests.Application.Services
{
  public class ProductServiceTests
  {
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<ILocationService> _mockLocationService;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
      _mockProductRepository = new Mock<IProductRepository>();
      _mockLocationService = new Mock<ILocationService>();
      _productService = new ProductService(_mockProductRepository.Object, _mockLocationService.Object);
    }

    #region Create Tests

    [Fact]
    public async Task Create_ValidProductCreateDto_ReturnsCreatedProductDto()
    {
      // Arrange
      var productCreateDto = new ProductCreateDto("Test Product", "Test Description", 99.99m, "test.jpg");
      var expectedProductDto = new ProductDto(
        Guid.NewGuid(),
        "Test Product",
        "Test Description",
        new Price(99.99m, "DKK"),
        "test.jpg",
        null
      );

      _mockProductRepository
        .Setup(repo => repo.Create(productCreateDto))
        .ReturnsAsync(expectedProductDto);

      // Act
      var result = await _productService.Create(productCreateDto);

      // Assert
      result.Should().BeEquivalentTo(expectedProductDto);
      _mockProductRepository.Verify(repo => repo.Create(productCreateDto), Times.Once);
    }

    #endregion

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenProductsExist_ReturnsAllProducts()
    {
      // Arrange
      var products = new List<ProductDto>
      {
        new ProductDto(Guid.NewGuid(), "Product 1", "Description 1", new Price(10.00m, "DKK"), null, null),
        new ProductDto(Guid.NewGuid(), "Product 2", "Description 2", new Price(20.00m, "DKK"), "image.jpg", null),
        new ProductDto(Guid.NewGuid(), "Product 3", "Description 3", new Price(30.00m, "DKK"), null, null)
      };

      _mockProductRepository
        .Setup(repo => repo.GetAll())
        .ReturnsAsync(products);

      // Act
      var result = await _productService.GetAll();

      // Assert
      result.Should().BeEquivalentTo(products);
      result.Should().HaveCount(3);
      _mockProductRepository.Verify(repo => repo.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenNoProductsExist_ReturnsEmptyCollection()
    {
      // Arrange
      _mockProductRepository
        .Setup(repo => repo.GetAll())
        .ReturnsAsync(new List<ProductDto>());

      // Act
      var result = await _productService.GetAll();

      // Assert
      result.Should().BeEmpty();
      _mockProductRepository.Verify(repo => repo.GetAll(), Times.Once);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_ExistingId_ReturnsProductDtoWithInventory()
    {
      // Arrange
      var productId = Guid.NewGuid();
      var baseProduct = new ProductDto(
        productId,
        "Test Product",
        "Test Description",
        new Price(99.99m, "DKK"),
        "test.jpg",
        null
      );

      var inventoryEntries = new List<InventoryEntryDto>
      {
        new InventoryEntryDto(productId, Guid.NewGuid(), 50),
        new InventoryEntryDto(productId, Guid.NewGuid(), 25)
      };

      var expectedProductWithInventory = new ProductDto(
        productId,
        "Test Product",
        "Test Description",
        new Price(99.99m, "DKK"),
        "test.jpg",
        inventoryEntries
      );

      _mockProductRepository
        .Setup(repo => repo.GetById(productId))
        .ReturnsAsync(baseProduct);

      _mockLocationService
        .Setup(service => service.GetInventoryOfProductAtAllLocations(productId))
        .ReturnsAsync(inventoryEntries);

      // Act
      var result = await _productService.GetById(productId);

      // Assert
      result.Should().BeEquivalentTo(expectedProductWithInventory);
      result!.Inventory.Should().HaveCount(2);
      _mockProductRepository.Verify(repo => repo.GetById(productId), Times.Once);
      _mockLocationService.Verify(service => service.GetInventoryOfProductAtAllLocations(productId), Times.Once);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNull()
    {
      // Arrange
      var productId = Guid.NewGuid();

      _mockProductRepository
        .Setup(repo => repo.GetById(productId))
        .ReturnsAsync((ProductDto?)null);

      // Act
      var result = await _productService.GetById(productId);

      // Assert
      result.Should().BeNull();
      _mockProductRepository.Verify(repo => repo.GetById(productId), Times.Once);
      _mockLocationService.Verify(service => service.GetInventoryOfProductAtAllLocations(It.IsAny<Guid>()), Times.Never);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_ValidProductUpdateDto_ReturnsUpdatedProductDto()
    {
      // Arrange
      var productId = Guid.NewGuid();
      var productUpdateDto = new ProductUpdateDto(
        productId,
        "Updated Product",
        "Updated Description",
        149.99m,
        "updated.jpg"
      );
      var expectedUpdatedProduct = new ProductDto(
        productId,
        "Updated Product",
        "Updated Description",
        new Price(149.99m, "DKK"),
        "updated.jpg",
        null
      );

      _mockProductRepository
        .Setup(repo => repo.Update(productUpdateDto))
        .ReturnsAsync(expectedUpdatedProduct);

      // Act
      var result = await _productService.Update(productUpdateDto);

      // Assert
      result.Should().BeEquivalentTo(expectedUpdatedProduct);
      _mockProductRepository.Verify(repo => repo.Update(productUpdateDto), Times.Once);
    }

    [Fact]
    public async Task Update_NonExistingProduct_ReturnsNull()
    {
      // Arrange
      var productId = Guid.NewGuid();
      var productUpdateDto = new ProductUpdateDto(
        productId,
        "Updated Product",
        "Updated Description",
        149.99m,
        "updated.jpg"
      );

      _mockProductRepository
        .Setup(repo => repo.Update(productUpdateDto))
        .ReturnsAsync((ProductDto?)null);

      // Act
      var result = await _productService.Update(productUpdateDto);

      // Assert
      result.Should().BeNull();
      _mockProductRepository.Verify(repo => repo.Update(productUpdateDto), Times.Once);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_ExistingId_ReturnsTrue()
    {
      // Arrange
      var productId = Guid.NewGuid();

      _mockProductRepository
        .Setup(repo => repo.Delete(productId))
        .ReturnsAsync(true);

      // Act
      var result = await _productService.Delete(productId);

      // Assert
      result.Should().BeTrue();
      _mockProductRepository.Verify(repo => repo.Delete(productId), Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsFalse()
    {
      // Arrange
      var productId = Guid.NewGuid();

      _mockProductRepository
        .Setup(repo => repo.Delete(productId))
        .ReturnsAsync(false);

      // Act
      var result = await _productService.Delete(productId);

      // Assert
      result.Should().BeFalse();
      _mockProductRepository.Verify(repo => repo.Delete(productId), Times.Once);
    }

    #endregion

    #region GetByCategoryId Tests

    [Fact]
    public async Task GetByCategoryId_ExistingCategory_ReturnsProductsInCategory()
    {
      // Arrange
      var categoryId = Guid.NewGuid();
      var productsInCategory = new List<ProductDto>
      {
        new ProductDto(Guid.NewGuid(), "Product 1", "Description 1", new Price(10.00m, "DKK"), null, null),
        new ProductDto(Guid.NewGuid(), "Product 2", "Description 2", new Price(20.00m, "DKK"), "image.jpg", null)
      };

      _mockProductRepository
        .Setup(repo => repo.GetByCategoryId(categoryId))
        .ReturnsAsync(productsInCategory);

      // Act
      var result = await _productService.GetByCategoryId(categoryId);

      // Assert
      result.Should().BeEquivalentTo(productsInCategory);
      result.Should().HaveCount(2);
      _mockProductRepository.Verify(repo => repo.GetByCategoryId(categoryId), Times.Once);
    }

    [Fact]
    public async Task GetByCategoryId_EmptyCategory_ReturnsEmptyCollection()
    {
      // Arrange
      var categoryId = Guid.NewGuid();

      _mockProductRepository
        .Setup(repo => repo.GetByCategoryId(categoryId))
        .ReturnsAsync(new List<ProductDto>());

      // Act
      var result = await _productService.GetByCategoryId(categoryId);

      // Assert
      result.Should().BeEmpty();
      _mockProductRepository.Verify(repo => repo.GetByCategoryId(categoryId), Times.Once);
    }

    [Fact]
    public async Task GetByCategoryId_NonExistingCategory_ReturnsEmptyCollection()
    {
      // Arrange
      var categoryId = Guid.NewGuid();

      _mockProductRepository
        .Setup(repo => repo.GetByCategoryId(categoryId))
        .ReturnsAsync(new List<ProductDto>());

      // Act
      var result = await _productService.GetByCategoryId(categoryId);

      // Assert
      result.Should().BeEmpty();
      _mockProductRepository.Verify(repo => repo.GetByCategoryId(categoryId), Times.Once);
    }

    #endregion
  }
}