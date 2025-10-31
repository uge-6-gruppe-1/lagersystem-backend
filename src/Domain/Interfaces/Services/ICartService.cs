using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Services
{
  public interface ICartService
  {
    Task<CartDto?> GetById(Guid cartId);
    Task<IEnumerable<CartDto>> GetByUserId(Guid userId);
    Task<CartDto> Create(CartCreateDto cartCreateDto);
    Task<CartDto?> Update(CartUpdateDto cartUpdateDto);
    Task<bool> Delete(Guid cartId);
    Task<CartDto?> UpdateProductQuantity(Guid cartId, Guid productId, QuantityChangeDto quantityChangeDto);
    Task<OrderDto?> ToOrder(Guid cartId);
  }
}