public class ProductDTO
{
    public string ID { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string ImagePath { get; set; }

    public ProductDTO(string id, string name, double price, string imagePath)
    {
        ID = id;
        Name = name;
        Price = price;
        ImagePath = imagePath;
    }
}