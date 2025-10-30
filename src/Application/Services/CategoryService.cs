using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;

namespace Backend.Application.Services
{
  public class CategoryService : ICategoryService
  {
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
      _categoryRepository = categoryRepository;
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