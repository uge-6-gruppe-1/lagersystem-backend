using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Repositories
{
  public interface ICartRepository
  {
    Task<CartDto> Create(CartCreateDto cart);
    Task<CartDto?> GetById(Guid id);
    Task<CartDto?> Update(CartUpdateDto cart);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<CartDto>> GetByUserId(Guid userId);
    Task<CartDto?> UpdateProductQuantity(Guid cartId, Guid productId, QuantityChangeDto quantityChangeDto);
  }
}