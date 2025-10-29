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

    public CategoryController(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
      var categoryDtos = await _categoryService.GetAll();
      return Ok(categoryDtos);
    }

    // GET: api/categories/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
      var categoryDto = await _categoryService.GetById(id);
      // Return 404 if not found
      if (categoryDto == null) return NotFound($"Category with ID {id} not found.");
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

    // DELETE: api/categories/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
      var deleted = await _categoryService.Delete(id);
      // Return 404 if not found
      if (!deleted) return NotFound($"Category with ID {id} not found.");
      // Return 204 No Content on successful deletion
      return NoContent();
    }

    // GET: api/categories/{id}/products
    [HttpGet("{id}/products")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(Guid id)
    {
      var productDtos = await _categoryService.GetProductsByCategoryId(id);
      // Return 200 with products
      return Ok(productDtos);
    }

    // PUT api/categories/{id}/products/{productId}
    [HttpPut("{id}/products/{productId}")]
    public async Task<ActionResult> AddProductToCategory(Guid id, Guid productId)
    {
      var result = await _categoryService.AddProductToCategory(id, productId);
      // Return 404 if category or product not found
      if (!result) return NotFound($"Category with ID {id} or Product with ID {productId} not found.");
      // Return 204 No Content on successful addition
      return NoContent();
    }

    // DELETE api/categories/{id}/products/{productId}
    [HttpDelete("{id}/products/{productId}")]
    public async Task<IActionResult> RemoveProductFromCategory(Guid id, Guid productId)
    {
      var result = await _categoryService.RemoveProductFromCategory(id, productId);
      // Return 404 if category or product not found
      if (!result) return NotFound($"Category with ID {id} or Product with ID {productId} not found.");
      // Return 204 No Content on successful removal
      return NoContent();
    }
  }
}