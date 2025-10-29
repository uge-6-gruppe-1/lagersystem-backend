using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Repositories
{
  public interface ICategoryRepository
  {
    Task<CategoryDto> Create(CategoryCreateDto categoryCreateDto);
    Task<IEnumerable<CategoryDto>> GetAll();
    Task<CategoryDto?> GetById(Guid id);
    Task<CategoryDto?> Update(CategoryUpdateDto categoryUpdateDto);
    Task<bool> Delete(Guid id);
    Task<bool> AddProductToCategory(Guid categoryId, Guid productId);
    Task<bool> RemoveProductFromCategory(Guid categoryId, Guid productId);
  }
}