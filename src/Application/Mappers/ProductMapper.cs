using Backend.Application.DTOs;
using Backend.Domain.Entities;
using Backend.Domain.ValueObjects;

namespace Backend.Application.Mappers
{
  public static class ProductMapper
  {
    public static ProductDto ToDto(this Product product)
    {
      return new ProductDto(
        product.Id,
        product.Name,
        product.Description,
        new Price(product.Price, "DKK"), // TODO: Replace hardcoded currency when multi-currency is supported
        product.ImagePath,
        null // Inventory not included by default
      );
    }

    public static Product ToEntity(this ProductDto productDto)
    {
      return new Product
      {
        Id = productDto.Id,
        Name = productDto.Name,
        Description = productDto.Description,
        Price = productDto.Price.Amount,
        ImagePath = productDto.ImagePath ?? string.Empty
      };
    }

    public static ProductDto PopulateInventory(this ProductDto productDto, IEnumerable<InventoryEntryDto> inventoryEntries)
    {
      return new ProductDto(
        productDto.Id,
        productDto.Name,
        productDto.Description,
        productDto.Price,
        productDto.ImagePath,
        inventoryEntries
      );
    }

    public static ProductDto ToDto(this ProductCreateDto createDto)
    {
      return new ProductDto(
        Guid.NewGuid(),
        createDto.Name,
        createDto.Description,
        createDto.Price,
        createDto.ImagePath,
        null // No inventory on creation
      );
    }

    public static Product ApplyUpdatesToEntity(this ProductUpdateDto updateDto, Product existingProduct)
    {
      existingProduct.Name = updateDto.Name ?? existingProduct.Name;
      existingProduct.Description = updateDto.Description ?? existingProduct.Description;
      existingProduct.Price = updateDto.Price?.Amount ?? existingProduct.Price;
      existingProduct.ImagePath = updateDto.ImagePath ?? existingProduct.ImagePath;
      return existingProduct;
    }
  }
}
