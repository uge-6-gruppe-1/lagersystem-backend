using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;
using Backend.Application.Mappers;

namespace Backend.Application.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _productRepository;
    private readonly ILocationService _locationService;

    public ProductService(IProductRepository productRepository, ILocationService locationService)
    {
      _productRepository = productRepository;
      _locationService = locationService;
    }

    public async Task<ProductDto> Create(ProductCreateDto productCreateDto)
    {
      var createdProductDto = await _productRepository.Create(productCreateDto);
      return createdProductDto;
    }

    public async Task<IEnumerable<ProductDto>> GetAll()
    {
      var productDtos = await _productRepository.GetAll();
      return productDtos;
    }

    public async Task<ProductDto?> GetById(Guid id)
    {
      var productDto = await _productRepository.GetById(id);
      if (productDto == null) return null;
      // Get inventory entries for this product across all locations and populate dto
      var inventoryEntries = await _locationService.GetInventoryOfProductAtAllLocations(id);
      productDto = productDto.PopulateInventory(inventoryEntries);
      return productDto;
    }

    public async Task<ProductDto?> Update(ProductUpdateDto productUpdateDto)
    {
      var updatedProductDto = await _productRepository.Update(productUpdateDto);
      return updatedProductDto;
    }
    
    public async Task<bool> Delete(Guid id)
    {
      return await _productRepository.Delete(id);
    }

    public async Task<IEnumerable<ProductDto>> GetByCategoryId(Guid categoryId)
    {
      return await _productRepository.GetByCategoryId(categoryId);
    }
  }
}