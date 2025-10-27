public class OrderLine
{
    public string ID { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public OrderLine(string id, Product product, int quantity)
    {
        ID = id;
        Product = product;
        Quantity = quantity;
    }
}