using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Services
{
  public interface IProductService
  {
    Task<ProductDto> Create(ProductCreateDto productCreateDto);
    Task<IEnumerable<ProductDto>> GetAll();
    Task<ProductDto?> GetById(Guid id);
    Task<ProductDto?> Update(ProductUpdateDto productUpdateDto);
    Task<bool> Delete(Guid id);
  }
}
