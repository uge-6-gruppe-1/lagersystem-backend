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
        product.ImagePath
      );
    }

    public static IEnumerable<ProductDto> ToDtos(this IEnumerable<Product> products)
    {
      return products.Select(p => p.ToDto());
    }

    public static Product ToEntity(this ProductDto productAppDto)
    {
      return new Product
      {
        Id = productAppDto.Id,
        Name = productAppDto.Name,
        Description = productAppDto.Description,
        Price = productAppDto.Price.Amount,
        ImagePath = productAppDto.ImagePath ?? string.Empty
      };
    }

    public static IEnumerable<Product> ToEntities(this IEnumerable<ProductDto> productAppDtos)
    {
      return productAppDtos.Select(dto => dto.ToEntity());
    }

    public static ProductDto ToDto(this ProductCreateDto createDto)
    {
      return new ProductDto(
        Guid.NewGuid(),
        createDto.Name,
        createDto.Description,
        createDto.Price,
        createDto.ImagePath
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
