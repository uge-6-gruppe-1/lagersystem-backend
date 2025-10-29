using Microsoft.EntityFrameworkCore;
using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Persistence.Contexts;
using Backend.Application.Mappers;
using Backend.Application.DTOs;

namespace Backend.Infrastructure.Persistence.Repositories
{
  public class ProductRepository(ApplicationDbContext context) : IProductRepository
  {
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<ProductDto>> GetAll()
    {
      return await _context.Product.Select(p => p.ToDto()).ToListAsync();
    }

    public async Task<ProductDto?> GetById(Guid id)
    {
      var product = await _context.Product.FindAsync(id);
      return product?.ToDto();
    }

    public async Task<ProductDto> Create(ProductCreateDto productCreateDto)
    {
      var productDto = productCreateDto.ToDto();
      _context.Product.Add(productDto.ToEntity());
      await _context.SaveChangesAsync();
      return productDto;
    }

    public async Task<ProductDto?> Update(ProductUpdateDto updateDto)
    {
      // Wrap in transaction for atomicity
      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        // Get existing product
        var existingProduct = await _context.Product
          .Where(p => p.Id == updateDto.Id)
          .FirstOrDefaultAsync();

        if (existingProduct == null)
        {
          await transaction.RollbackAsync();
          return null;
        }

        // Apply updates
        existingProduct = updateDto.ApplyUpdatesToEntity(existingProduct);

        // Save changes
        _context.Product.Update(existingProduct);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return existingProduct.ToDto();
      }
      catch
      {
        // Rollback transaction on error
        await transaction.RollbackAsync();
        throw;
      }
    }

    public async Task<bool> Delete(Guid id)
    {
      var product = await _context.Product.FindAsync(id);
      if (product == null)
        return false;

      _context.Product.Remove(product);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Exists(Guid id)
    {
      return await _context.Product.AnyAsync(p => p.Id == id);
    }
  }
}