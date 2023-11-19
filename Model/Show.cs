namespace Worker.Model;

public class Show
{
    public Show () { }

    public Show (int id, string name, string description, DateTime date, string local, Category? categorty)
    {
        Id = id;
        Name = name;
        Description = description;
        Date = date;
        Local = local;
        Categorty = categorty;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }

    public string Local { get; set; }

    public virtual Category? Categorty { get; set; }
}
