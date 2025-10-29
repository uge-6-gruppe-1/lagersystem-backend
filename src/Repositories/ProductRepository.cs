using System.Collections.Generic;

public class ProductRepository
{
    // Placeholder for product data (store domain Product so service can map to DTO)
    private readonly IEnumerable<Product> _products = new List<Product>
    {
        new Product("1", "Laptop", 9999.99, "/images/laptop.png"),
        new Product("2", "Smartphone", 4999.99, "/images/smartphone.png"),
        new Product("3", "Tablet", 2999.99, "/images/tablet.png"),
        new Product("4", "Smartwatch", 1999.99, "/images/smartwatch.png"),
        new Product("5", "Headphones", 999.99, "/images/headphones.png"),
        new Product("6", "Camera", 5999.99, "/images/camera.png"),
        new Product("7", "Printer", 1499.99, "/images/printer.png"),
        new Product("8", "Monitor", 2499.99, "/images/monitor.png"),
        new Product("9", "Keyboard", 499.99, "/images/keyboard.png"),
        new Product("10", "Mouse", 299.99, "/images/mouse.png")
    };

    public IEnumerable<Product> GetAllProducts()
    {
        return _products;
    }
}