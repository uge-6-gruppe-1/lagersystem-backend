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
}