using System.Collections.Generic;
using System.Linq;

public class ProductService
{
    public IEnumerable<ProductDTO> MapProducts(IEnumerable<Product> products)
    {
        return products.Select(p => new ProductDTO(
            p.ID,
            p.Name,
            p.Price,
            p.ImagePath
        ));
    }
}