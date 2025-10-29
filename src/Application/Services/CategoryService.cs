using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;

namespace Backend.Application.Services
{
  public class CategoryService : ICategoryService
  {
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
      _categoryRepository = categoryRepository;
      _productRepository = productRepository;
    }

    public async Task<CategoryDto> Create(CategoryCreateDto categoryCreateDto)
    {
      return await _categoryRepository.Create(categoryCreateDto);
    }

    public async Task<IEnumerable<CategoryDto>> GetAll()
    {
      return await _categoryRepository.GetAll();
    }

    public async Task<CategoryDto?> GetById(Guid id)
    {
      return await _categoryRepository.GetById(id);
    }

    public async Task<CategoryDto?> Update(CategoryUpdateDto categoryUpdateDto)
    {
      return await _categoryRepository.Update(categoryUpdateDto);
    }

    public async Task<bool> Delete(Guid id)
    {
      return await _categoryRepository.Delete(id);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryId(Guid categoryId)
    {
      return await _productRepository.GetByCategoryId(categoryId);
    }

    public async Task<bool> AddProductToCategory(Guid categoryId, Guid productId)
    {
      return await _categoryRepository.AddProductToCategory(categoryId, productId);
    }

    public async Task<bool> RemoveProductFromCategory(Guid categoryId, Guid productId)
    {
      return await _categoryRepository.RemoveProductFromCategory(categoryId, productId);
    }
  }
}