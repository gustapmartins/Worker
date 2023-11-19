namespace Worker.Model;

public  class Category
{
    public Category() { }

    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
