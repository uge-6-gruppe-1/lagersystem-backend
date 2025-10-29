using Microsoft.EntityFrameworkCore;
using Backend.Domain.Interfaces.Repositories;
using Backend.Infrastructure.Persistence.Contexts;
using Backend.Application.Mappers;
using Backend.Application.DTOs;

namespace Backend.Infrastructure.Persistence.Repositories
{
  public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
  {
    private readonly ApplicationDbContext _context = context;

    public async Task<CategoryDto> Create(CategoryCreateDto categoryCreateDto)
    {
      var categoryDto = categoryCreateDto.ToDto();
      _context.Category.Add(categoryDto.ToEntity());
      await _context.SaveChangesAsync();
      return categoryDto;
    }

    public async Task<IEnumerable<CategoryDto>> GetAll()
    {
      var categories = await _context.Category.ToListAsync();
      return categories.Select(c => c.ToDto());
    }

    public async Task<CategoryDto?> GetById(Guid id)
    {
      var category = await _context.Category.FindAsync(id);
      return category?.ToDto();
    }

    public async Task<CategoryDto?> Update(CategoryUpdateDto categoryUpdateDto)
    {
      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        // Get existing category
        var existingCategory = await _context.Category
          .Where(c => c.Id == categoryUpdateDto.Id)
          .FirstOrDefaultAsync();

        if (existingCategory == null)
        {
          await transaction.RollbackAsync();
          return null;
        }

        // Apply updates
        existingCategory = categoryUpdateDto.ApplyUpdatesToEntity(existingCategory);

        // Save changes
        _context.Category.Update(existingCategory);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return existingCategory.ToDto();
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
      var category = await _context.Category.FindAsync(id);
      if (category == null)
      {
        return false;
      }

      _context.Category.Remove(category);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> AddProductToCategory(Guid categoryId, Guid productId)
    {
      var category = await _context.Category
        .Include(c => c.Products)
        .FirstOrDefaultAsync(c => c.Id == categoryId);
      var product = await _context.Product.FindAsync(productId);

      if (category == null || product == null)
        return false;

      category.Products.Add(product);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> RemoveProductFromCategory(Guid categoryId, Guid productId)
    {
      var category = await _context.Category
        .Include(c => c.Products)
        .FirstOrDefaultAsync(c => c.Id == categoryId);
      var product = await _context.Product.FindAsync(productId);

      if (category == null || product == null)
        return false;

      category.Products.Remove(product);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}