using Backend.Application.DTOs;
using Backend.Domain.Entities;

namespace Backend.Application.Mappers
{
  public static class CategoryMapper
  {
    public static CategoryDto ToDto(this Category category)
    {
      return new CategoryDto(
        category.Id,
        category.Name,
        category.Description
      );
    }

    public static Category ToEntity(this CategoryDto dto)
    {
      return new Category
      {
        Id = dto.Id,
        Name = dto.Name,
        Description = dto.Description ?? string.Empty
      };
    }

    public static CategoryDto ToDto(this CategoryCreateDto createDto)
    {
      return new CategoryDto(
        Guid.NewGuid(),
        createDto.Name,
        createDto.Description
      );
    }
    public static Category ApplyUpdatesToEntity(this CategoryUpdateDto updateDto, Category existingCategory)
    {
      existingCategory.Name = updateDto.Name ?? existingCategory.Name;
      existingCategory.Description = updateDto.Description ?? existingCategory.Description;
      return existingCategory;
    }
  }
}