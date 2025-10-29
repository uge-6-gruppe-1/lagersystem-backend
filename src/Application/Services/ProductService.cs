using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;

namespace Backend.Application.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
      _productRepository = productRepository;
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
  }
}