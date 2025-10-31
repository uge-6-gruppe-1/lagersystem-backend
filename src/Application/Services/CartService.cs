using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;

namespace Backend.Application.Services
{
  public class CartService : ICartService
  {
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
      _cartRepository = cartRepository;
    }

    public async Task<CartDto> Create(CartCreateDto cartCreateDto)
    {
      var createdCart = await _cartRepository.Create(cartCreateDto);
      return createdCart;
    }

    public async Task<bool> Delete(Guid cartId)
    {
      var result = await _cartRepository.Delete(cartId);
      return result;
    }

    public async Task<CartDto?> GetById(Guid cartId)
    {
      var cart = await _cartRepository.GetById(cartId);
      return cart;
    }

    public async Task<IEnumerable<CartDto>> GetByUserId(Guid userId)
    {
      var carts = await _cartRepository.GetByUserId(userId);
      return carts;
    }

    public async Task<CartDto?> Update(CartUpdateDto cartUpdateDto)
    {
      var updatedCart = await _cartRepository.Update(cartUpdateDto);
      return updatedCart;
    }

    public async Task<CartDto?> UpdateProductQuantity(Guid cartId, Guid productId, QuantityChangeDto quantityChangeDto)
    {
      var updatedCart = await _cartRepository.UpdateProductQuantity(cartId, productId, quantityChangeDto);
      return updatedCart;
    }
  }
}