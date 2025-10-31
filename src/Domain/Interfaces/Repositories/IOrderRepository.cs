using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Repositories
{
  public interface IOrderRepository
  {
    Task<OrderDto> Create(OrderCreateDto order);
    Task<IEnumerable<OrderDto>> GetAll();
    Task<OrderDto?> GetById(Guid id);
    Task<OrderDto?> Update(OrderUpdateDto order);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<OrderDto>> GetByUserId(Guid userId);
    Task<OrderDto?> UpdateProductQuantity(Guid orderId, Guid productId, QuantityChangeDto quantityChangeDto);
  }
}