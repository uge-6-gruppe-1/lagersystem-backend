using Moq;
using Xunit;
using FluentAssertions;
using Backend.Application.Services;
using Backend.Domain.Interfaces.Repositories;
using Backend.Application.DTOs;

namespace Backend.Tests.Application.Services
{
  public class CategoryServiceTests
  {
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
      _mockCategoryRepository = new Mock<ICategoryRepository>();
      _categoryService = new CategoryService(_mockCategoryRepository.Object);
    }

    #region Create Tests

    [Fact]
    public async Task Create_ValidCategoryCreateDto_ReturnsCreatedCategoryDto()
    {
      // Arrange
      var categoryCreateDto = new CategoryCreateDto("Test Category", "Test Description");
      var expectedCategoryDto = new CategoryDto(Guid.NewGuid(), "Test Category", "Test Description");

      _mockCategoryRepository
        .Setup(repo => repo.Create(categoryCreateDto))
        .ReturnsAsync(expectedCategoryDto);

      // Act
      var result = await _categoryService.Create(categoryCreateDto);

      // Assert
      result.Should().BeEquivalentTo(expectedCategoryDto);
      _mockCategoryRepository.Verify(repo => repo.Create(categoryCreateDto), Times.Once);
    }

    #endregion

    #region GetAll Tests

    [Fact]
    public async Task GetAll_WhenCategoriesExist_ReturnsAllCategories()
    {
      // Arrange
      var categories = new List<CategoryDto>
      {
        new CategoryDto(Guid.NewGuid(), "Category 1", "Description 1"),
        new CategoryDto(Guid.NewGuid(), "Category 2", "Description 2"),
        new CategoryDto(Guid.NewGuid(), "Category 3", null)
      };

      _mockCategoryRepository
        .Setup(repo => repo.GetAll())
        .ReturnsAsync(categories);

      // Act
      var result = await _categoryService.GetAll();

      // Assert
      result.Should().BeEquivalentTo(categories);
      result.Should().HaveCount(3);
      _mockCategoryRepository.Verify(repo => repo.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenNoCategoriesExist_ReturnsEmptyCollection()
    {
      // Arrange
      _mockCategoryRepository
        .Setup(repo => repo.GetAll())
        .ReturnsAsync(new List<CategoryDto>());

      // Act
      var result = await _categoryService.GetAll();

      // Assert
      result.Should().BeEmpty();
      _mockCategoryRepository.Verify(repo => repo.GetAll(), Times.Once);
    }

    #endregion

    #region GetById Tests

    [Fact]
    public async Task GetById_ExistingId_ReturnsCategoryDto()
    {
      // Arrange
      var categoryId = Guid.NewGuid();
      var expectedCategory = new CategoryDto(categoryId, "Test Category", "Test Description");

      _mockCategoryRepository
        .Setup(repo => repo.GetById(categoryId))
        .ReturnsAsync(expectedCategory);

      // Act
      var result = await _categoryService.GetById(categoryId);

      // Assert
      result.Should().BeEquivalentTo(expectedCategory);
      _mockCategoryRepository.Verify(repo => repo.GetById(categoryId), Times.Once);
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNull()
    {
      // Arrange
      var categoryId = Guid.NewGuid();

      _mockCategoryRepository
        .Setup(repo => repo.GetById(categoryId))
        .ReturnsAsync((CategoryDto?)null);

      // Act
      var result = await _categoryService.GetById(categoryId);

      // Assert
      result.Should().BeNull();
      _mockCategoryRepository.Verify(repo => repo.GetById(categoryId), Times.Once);
    }

    #endregion

    #region Update Tests

    [Fact]
    public async Task Update_ValidCategoryUpdateDto_ReturnsUpdatedCategoryDto()
    {
      // Arrange
      var categoryId = Guid.NewGuid();
      var categoryUpdateDto = new CategoryUpdateDto(categoryId, "Updated Category", "Updated Description");
      var expectedUpdatedCategory = new CategoryDto(categoryId, "Updated Category", "Updated Description");

      _mockCategoryRepository
        .Setup(repo => repo.Update(categoryUpdateDto))
        .ReturnsAsync(expectedUpdatedCategory);

      // Act
      var result = await _categoryService.Update(categoryUpdateDto);

      // Assert
      result.Should().BeEquivalentTo(expectedUpdatedCategory);
      _mockCategoryRepository.Verify(repo => repo.Update(categoryUpdateDto), Times.Once);
    }

    [Fact]
    public async Task Update_NonExistingCategory_ReturnsNull()
    {
      // Arrange
      var categoryId = Guid.NewGuid();
      var categoryUpdateDto = new CategoryUpdateDto(categoryId, "Updated Category", "Updated Description");

      _mockCategoryRepository
        .Setup(repo => repo.Update(categoryUpdateDto))
        .ReturnsAsync((CategoryDto?)null);

      // Act
      var result = await _categoryService.Update(categoryUpdateDto);

      // Assert
      result.Should().BeNull();
      _mockCategoryRepository.Verify(repo => repo.Update(categoryUpdateDto), Times.Once);
    }

    #endregion

    #region Delete Tests

    [Fact]
    public async Task Delete_ExistingId_ReturnsTrue()
    {
      // Arrange
      var categoryId = Guid.NewGuid();

      _mockCategoryRepository
        .Setup(repo => repo.Delete(categoryId))
        .ReturnsAsync(true);

      // Act
      var result = await _categoryService.Delete(categoryId);

      // Assert
      result.Should().BeTrue();
      _mockCategoryRepository.Verify(repo => repo.Delete(categoryId), Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsFalse()
    {
      // Arrange
      var categoryId = Guid.NewGuid();

      _mockCategoryRepository
        .Setup(repo => repo.Delete(categoryId))
        .ReturnsAsync(false);

      // Act
      var result = await _categoryService.Delete(categoryId);

      // Assert
      result.Should().BeFalse();
      _mockCategoryRepository.Verify(repo => repo.Delete(categoryId), Times.Once);
    }

    #endregion

    #region AddProductToCategory Tests

    [Fact]
    public async Task AddProductToCategory_ValidIds_ReturnsTrue()
    {
      // Arrange
      var categoryId = Guid.NewGuid();
      var productId = Guid.NewGuid();

      _mockCategoryRepository
        .Setup(repo => repo.AddProductToCategory(categoryId, productId))
        .ReturnsAsync(true);

      // Act
      var result = await _categoryService.AddProductToCategory(categoryId, productId);

      // Assert
      result.Should().BeTrue();
      _mockCategoryRepository.Verify(repo => repo.AddProductToCategory(categoryId, productId), Times.Once);
    }

    [Fact]
    public async Task AddProductToCategory_InvalidIds_ReturnsFalse()
    {
      // Arrange
      var categoryId = Guid.NewGuid();
      var productId = Guid.NewGuid();

      _mockCategoryRepository
        .Setup(repo => repo.AddProductToCategory(categoryId, productId))
        .ReturnsAsync(false);

      // Act
      var result = await _categoryService.AddProductToCategory(categoryId, productId);

      // Assert
      result.Should().BeFalse();
      _mockCategoryRepository.Verify(repo => repo.AddProductToCategory(categoryId, productId), Times.Once);
    }

    #endregion

    #region RemoveProductFromCategory Tests

    [Fact]
    public async Task RemoveProductFromCategory_ValidIds_ReturnsTrue()
    {
      // Arrange
      var categoryId = Guid.NewGuid();
      var productId = Guid.NewGuid();

      _mockCategoryRepository
        .Setup(repo => repo.RemoveProductFromCategory(categoryId, productId))
        .ReturnsAsync(true);

      // Act
      var result = await _categoryService.RemoveProductFromCategory(categoryId, productId);

      // Assert
      result.Should().BeTrue();
      _mockCategoryRepository.Verify(repo => repo.RemoveProductFromCategory(categoryId, productId), Times.Once);
    }

    [Fact]
    public async Task RemoveProductFromCategory_InvalidIds_ReturnsFalse()
    {
      // Arrange
      var categoryId = Guid.NewGuid();
      var productId = Guid.NewGuid();

      _mockCategoryRepository
        .Setup(repo => repo.RemoveProductFromCategory(categoryId, productId))
        .ReturnsAsync(false);

      // Act
      var result = await _categoryService.RemoveProductFromCategory(categoryId, productId);

      // Assert
      result.Should().BeFalse();
      _mockCategoryRepository.Verify(repo => repo.RemoveProductFromCategory(categoryId, productId), Times.Once);
    }

    #endregion
  }
}