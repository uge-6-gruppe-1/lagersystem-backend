using Backend.Application.DTOs;
using Backend.Domain.Entities;

namespace Backend.Application.Mappers
{
  public static class OrderLineMapper
  {
    public static OrderLineDto ToDto(this OrderLine orderLine)
    {
      return new OrderLineDto
      {
        Id = orderLine.Id,
        ProductId = orderLine.ProductId,
        Quantity = orderLine.Quantity
      };
    }

    public static OrderLine ToEntity(this OrderLineDto orderLineDto)
    {
      return new OrderLine
      {
        Id = orderLineDto.Id,
        ProductId = orderLineDto.ProductId,
        Quantity = orderLineDto.Quantity
      };
    }
  }
}