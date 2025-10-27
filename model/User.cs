public class User
{
    string ID { get; set; }
    string Name { get; set; }

    public User(string id, string name)
    {
        ID = id;
        Name = name;
    }
}