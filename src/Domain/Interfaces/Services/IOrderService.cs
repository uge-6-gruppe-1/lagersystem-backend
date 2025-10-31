using Backend.Application.DTOs;

namespace Backend.Domain.Interfaces.Services
{
  public interface IOrderService
  {
    Task<OrderDto> Create(OrderCreateDto orderCreateDto);
    Task<IEnumerable<OrderDto>> GetAll();
    Task<OrderDto?> GetById(Guid id);
    Task<OrderDto?> Update(OrderUpdateDto orderUpdateDto);
    Task<bool> Delete(Guid id);
    Task<IEnumerable<OrderDto>> GetByUserId(Guid userId);
  }