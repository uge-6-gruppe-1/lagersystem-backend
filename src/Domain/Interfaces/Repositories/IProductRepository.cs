using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Repositories
{
  public interface IProductRepository
  {
    Task<ProductDto> Create(ProductCreateDto product);
    Task<IEnumerable<ProductDto>> GetAll();
    Task<ProductDto?> GetById(Guid id);
    Task<ProductDto?> Update(ProductUpdateDto product);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<ProductDto>> GetByCategoryId(Guid categoryId);
  }
}