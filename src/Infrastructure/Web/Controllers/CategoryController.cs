using Microsoft.AspNetCore.Mvc;
using Backend.Domain.Interfaces.Services;
using Backend.Application.DTOs;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/categories")]
  public class CategoryController : ControllerBase
  {
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public CategoryController(ICategoryService categoryService, IProductService productService)
    {
      _categoryService = categoryService;
      _productService = productService;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
      var categoryDtos = await _categoryService.GetAll();
      return Ok(categoryDtos);
    }

    // GET: api/categories/{categoryId}
    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid categoryId)
    {
      var categoryDto = await _categoryService.GetById(categoryId);
      // Return 404 if not found
      if (categoryDto == null) return NotFound($"Category with ID {categoryId} not found.");
      // Return 200 with category
      return Ok(categoryDto);
    }

    // POST: api/categories/
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
    {
      // Return 400 if request validation fails
      if (!ModelState.IsValid) return BadRequest(ModelState);
      var createdCategoryDto = await _categoryService.Create(categoryCreateDto);
      // Return 201 with location header
      return CreatedAtAction(nameof(GetCategory), new { id = createdCategoryDto.Id }, createdCategoryDto);
    }

    // PATCH: api/categories/
    [HttpPatch]
    public async Task<ActionResult<CategoryDto>> UpdateCategory([FromBody] CategoryUpdateDto request)
    {
      // Return 400 if request validation fails
      if (!ModelState.IsValid) return BadRequest(ModelState);
      var updatedCategory = await _categoryService.Update(request);
      // Return 404 if not found
      if (updatedCategory == null) return NotFound($"Category with ID {request.Id} not found.");
      // Return 200 with updated category
      return Ok(updatedCategory);
    }

    // DELETE: api/categories/{categoryId}
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategory(Guid categoryId)
    {
      var deleted = await _categoryService.Delete(categoryId);
      // Return 404 if not found
      if (!deleted) return NotFound($"Category with ID {categoryId} not found.");
      // Return 204 No Content on successful deletion
      return NoContent();
    }

    // GET: api/categories/{categoryId}/products
    [HttpGet("{categoryId}/products")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(Guid categoryId)
    {
      var productDtos = await _productService.GetByCategoryId(categoryId);
      // Return 200 with products
      return Ok(productDtos);
    }

    // PUT api/categories/{categoryId}/products/{productId}
    [HttpPut("{categoryId}/products/{productId}")]
    public async Task<ActionResult> AddProductToCategory(Guid categoryId, Guid productId)
    {
      var result = await _categoryService.AddProductToCategory(categoryId, productId);
      // Return 404 if category or product not found
      if (!result) return NotFound($"Category with ID {categoryId} or Product with ID {productId} not found.");
      // Return 204 No Content on successful addition
      return NoContent();
    }

    // DELETE api/categories/{categoryId}/products/{productId}
    [HttpDelete("{categoryId}/products/{productId}")]
    public async Task<IActionResult> RemoveProductFromCategory(Guid categoryId, Guid productId)
    {
      var result = await _categoryService.RemoveProductFromCategory(categoryId, productId);
      // Return 404 if category or product not found
      if (!result) return NotFound($"Category with ID {categoryId} or Product with ID {productId} not found.");
      // Return 204 No Content on successful removal
      return NoContent();
    }
  }
}