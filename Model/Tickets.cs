using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worker.Model;

public class Tickets
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int QuantityTickets { get; set; }

    public decimal Price { get; set; }

    public virtual Show? Show { get; set; }
}
