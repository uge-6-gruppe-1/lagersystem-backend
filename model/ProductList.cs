public class ProductList
{
    string ID { get; set; }
    string? UserID { get; set; }
    List<CartItem> Items { get; set; }

    public ProductList(string id, string? userID, List<CartItem> items)
    {
        ID = id;
        UserID = userID;
        Items = items;
    }

    public int GetQuantity()
    {
        return Items.Sum(item => item.Quantity);
    }

    public Price GetTotal(string currencySymbol = "DKK")
    {
        double totalAmount = 0;
        foreach (var item in Items)
        {
            totalAmount += item.Product.Price.Amount * item.Quantity;
        }
        return new Price(totalAmount, currencySymbol);
    }
}