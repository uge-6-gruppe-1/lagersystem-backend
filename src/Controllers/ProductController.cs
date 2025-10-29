using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    [HttpGet]
    public IEnumerable<ProductDTO> GetAllProducts()
    {
        var repository = new ProductRepository();
        var service = new ProductService();

        var products = repository.GetAllProducts();
        return service.MapProducts(products);
	}

    [HttpGet]
    [Route("{id}")]
    public ActionResult<ProductDTO> GetProductById(string id)
    {
        var repository = new ProductRepository();
        var service = new ProductService();
        var products = repository.GetAllProducts();
        var product = products.FirstOrDefault(p => p.ID == id);
        if (product == null)
        {
            return NotFound();
        }
        var productDTO = service.MapProducts(new List<Product> { product }).FirstOrDefault();
        return Ok(productDTO);
	}
}