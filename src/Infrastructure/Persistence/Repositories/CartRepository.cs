using Microsoft.EntityFrameworkCore;
using Backend.Domain.Interfaces.Repositories;
using Backend.Infrastructure.Persistence.Contexts;
using Backend.Application.Mappers;
using Backend.Application.DTOs;
using Backend.Domain.Entities;
using Backend.Domain.Enums;

namespace Backend.Infrastructure.Persistence.Repositories
{
  public class CartRepository : ICartRepository
  {
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<CartDto> Create(CartCreateDto cartCreateDto)
    {
      var cartDto = cartCreateDto.ToDto();
      _context.Cart.Add(cartDto.ToEntity());
      await _context.SaveChangesAsync();
      return cartDto;
    }

    public async Task<bool> Delete(Guid id)
    {
      var cart = await _context.Cart.FindAsync(id);
      if (cart == null) return false;
      _context.Cart.Remove(cart);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<CartDto?> GetById(Guid id)
    {
      var cart = await _context.Cart.FindAsync(id);
      return cart?.ToDto();
    }

    public async Task<IEnumerable<CartDto>> GetByUserId(Guid userId)
    {
      var carts = await _context.Cart
        .Where(c => c.UserId == userId)
        .ToListAsync();
      return carts.Select(c => c.ToDto());
    }

    public async Task<CartDto?> Update(CartUpdateDto cart)
    {
      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        // Get existing cart
        var existingCart = await _context.Cart
          .Where(c => c.Id == cart.Id)
          .FirstOrDefaultAsync();

        if (existingCart == null)
        {
          await transaction.RollbackAsync();
          return null;
        }

        // Apply updates
        existingCart = cart.ApplyUpdatesToEntity(existingCart);

        // Save changes
        _context.Cart.Update(existingCart);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return existingCart.ToDto();
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }

    public async Task<CartDto?> UpdateProductQuantity(Guid cartId, Guid productId, QuantityChangeDto quantityChangeDto)
    {
      using var transaction = await _context.Database.BeginTransactionAsync();
      try
      {
        // Get existing cart
        var existingCart = await _context.Cart
          .Where(c => c.Id == cartId)
          .FirstOrDefaultAsync();

        if (existingCart == null)
        {
          // Cart does not exist, rollback and return null
          await transaction.RollbackAsync();
          return null;
        }

        // Get existing order line
        var orderLine = existingCart.OrderLines
          .FirstOrDefault(ol => ol.ProductId == productId);
        if (orderLine == null)
        {
          // Order line does not exist, create new order line with initial quantity 0
          orderLine = new OrderLine
          {
            Id = Guid.NewGuid(),
            ProductId = productId,
            Quantity = 0
          };
          existingCart.OrderLines.Add(orderLine);
        }
        // Apply quantity change
        if (quantityChangeDto.operation == AdjustmentType.INCREMENT)
        {
          orderLine.Quantity += quantityChangeDto.amount;
        }
        else if (quantityChangeDto.operation == AdjustmentType.DECREMENT)
        {
          orderLine.Quantity -= quantityChangeDto.amount;
        }
        else if (quantityChangeDto.operation == AdjustmentType.SET)
        {
          orderLine.Quantity = quantityChangeDto.amount;
        }

        if (orderLine.Quantity <= 0)
        {
          // Remove order line if quantity is zero or negative
          existingCart.OrderLines.Remove(orderLine);
        }

        // Save changes
        _context.Cart.Update(existingCart);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return existingCart.ToDto();
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }
  }
}