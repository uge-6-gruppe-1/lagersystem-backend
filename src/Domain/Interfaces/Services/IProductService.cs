using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Services
{
  public interface IProductService
  {
    Task<IEnumerable<ProductDto>> GetAll();
    Task<ProductDto?> GetById(Guid id);
    Task<ProductDto> Create(ProductCreateDto productCreateDto);
    Task<ProductDto?> Update(ProductUpdateDto productUpdateDto);
    Task<bool> Delete(Guid id);
  }
}
