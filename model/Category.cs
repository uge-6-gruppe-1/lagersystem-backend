public class Category
{
    string ID { get; set; }
    string Name { get; set; }
    string Description { get; set; }

    public Category(string id, string name, string description)
    {
        ID = id;
        Name = name;
        Description = description;
    }
}