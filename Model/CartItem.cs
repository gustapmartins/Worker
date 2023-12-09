using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Worker.Enum;

namespace Worker.Model;

public class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    public int Quantity { get; set; }

    public virtual Tickets Ticket { get; set; }

    public StatusPayment statusPayment { get; set; }
}
