using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public string ID { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string ImagePath { get; set; }

    public Product(string id, string name, double price, string imagePath)
    {
        ID = id;
        Name = name;
        Price = price;
        ImagePath = imagePath;
    }
}