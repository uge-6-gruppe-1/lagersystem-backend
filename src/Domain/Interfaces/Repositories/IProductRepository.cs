using Backend.Application.DTOs;
using Backend.Domain.Entities;

namespace Backend.Domain.Interfaces.Repositories
{
  public interface IProductRepository
  {
    Task<IEnumerable<ProductDto>> GetAll();
    Task<ProductDto?> GetById(Guid id);
    Task<ProductDto> Create(ProductCreateDto product);
    Task<ProductDto?> Update(ProductUpdateDto product);
    Task<bool> Delete(Guid id);
    Task<bool> Exists(Guid id);
  }
}