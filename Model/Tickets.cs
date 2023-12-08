using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worker.Model;

public class Tickets
{

    [Key]
    public string Id { get; set; }

    public int QuantityTickets { get; set; }

    public decimal Price { get; set; }

    public virtual Show? Show { get; set; }
}
