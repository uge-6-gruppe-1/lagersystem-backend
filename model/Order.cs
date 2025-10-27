public class Order : ProductList
{
    string ShippingDetailsID { get; set; }

    public Order(string id, string? userID, List<OrderLine> items, string shippingDetailsID) : base(id, userID, items)
    {
        ShippingDetailsID = shippingDetailsID;
    }
}