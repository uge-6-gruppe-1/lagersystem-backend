using Backend.Application.DTOs;
using Backend.Domain.Entities;

namespace Backend.Application.Mappers
{
  public static class CartMapper
  {
    public static CartDto ToDto(this Cart cart)
    {
      return new CartDto
      {
        Id = cart.Id,
        UserId = cart.UserId,
        Name = cart.Name,
        Items = cart.OrderLines.Select(item => item.ToDto()).ToList()
      };
    }

    public static Cart ToEntity(this CartDto cartDto)
    {
      return new Cart
      {
        Id = cartDto.Id,
        UserId = cartDto.UserId,
        Name = cartDto.Name,
        OrderLines = cartDto.Items.Select(item => item.ToEntity()).ToList()
      };
    }

    public static CartDto ToDto(this CartCreateDto cartCreateDto)
    {
      return new CartDto
      {
        Id = Guid.NewGuid(),
        UserId = cartCreateDto.UserId,
        Name = cartCreateDto.Name
      };
    }

    public static Cart ApplyUpdatesToEntity(this CartUpdateDto cartUpdateDto, Cart existingCart)
    {
      existingCart.UserId = cartUpdateDto.UserId;
      existingCart.Name = cartUpdateDto.Name;
      return existingCart;
    }
  }
}