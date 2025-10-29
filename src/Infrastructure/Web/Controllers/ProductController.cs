using Microsoft.AspNetCore.Mvc;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/products")]
  public class ProductController : ControllerBase
  {
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
      _productService = productService;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
      var productDtos = await _productService.GetAll();
      // Return 200 with products
      return Ok(productDtos);
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
    {
      var productDto = await _productService.GetById(id);
      // Return 404 if not found
      if (productDto == null) return NotFound($"Product with ID {id} not found.");
      // Return 200 with product
      return Ok(productDto);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductCreateDto productCreateDto)
    {
      // Return 400 if request validation fails
      if (!ModelState.IsValid) return BadRequest(ModelState);
      var createdProduct = await _productService.Create(productCreateDto);
      // Return 201 with location header
      return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    // PATCH: api/products/
    [HttpPatch]
    public async Task<ActionResult<ProductDto>> UpdateProduct([FromBody] ProductUpdateDto productUpdateDto)
    {
      // Return 400 if request validation fails
      if (!ModelState.IsValid) return BadRequest(ModelState);
      var updatedProduct = await _productService.Update(productUpdateDto);
      // Return 404 if not found
      if (updatedProduct == null) return NotFound($"Product with ID {productUpdateDto.Id} not found.");
      // Return 200 with updated product
      return Ok(updatedProduct);
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
      var deleted = await _productService.Delete(id);
      // Return 404 if not found
      if (!deleted) return NotFound($"Product with ID {id} not found.");
      // Return 204 No Content on successful deletion
      return NoContent();
    }
  }
}