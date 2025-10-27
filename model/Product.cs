public class Product
{
    string ID { get; set; }
    string Name { get; set; }
    Price Price { get; set; }
    string ImagePath { get; set; }
    IEnumerable<InventoryEntry> Inventory { get; set; }
    IEnumerable<string> CategoryIDs { get; set; }

    public Product(string id, string name, Price price, string imagePath, IEnumerable<InventoryEntry> inventory, IEnumerable<string> categoryIDs)
    {
        ID = id;
        Name = name;
        Price = price;
        ImagePath = imagePath;
        Inventory = inventory;
        CategoryIDs = categoryIDs;
    }
}
