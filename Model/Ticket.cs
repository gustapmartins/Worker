namespace Worker.Model;

internal class Ticket
{
    public Ticket() { }

    public Ticket(int id, int quantityTickets, decimal price, Show? show)
    {
        Id = id;
        QuantityTickets = quantityTickets;
        Price = price;
        Show = show;
    }

    public int Id { get; set; }

    public int QuantityTickets { get; set; }

    public decimal Price { get; set; }

    public virtual Show? Show { get; set; }
}
