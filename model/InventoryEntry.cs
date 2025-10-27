public class InventoryEntry
{
    string ProductID { get; set; }
    string LocationID { get; set; }
    int Available { get; set; }
    int Reserved { get; set; }

    public InventoryEntry(string productID, string locationID, int available, int reserved)
    {
        ProductID = productID;
        LocationID = locationID;
        Available = available;
        Reserved = reserved;
    }

    public bool IsInStock()
    {
        return Available > 0;
    }
}