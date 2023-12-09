using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Worker.Enum;

namespace Worker.Model;

public class Carts

{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    public virtual List<CartItem> CartList { get; set; }

    public virtual Users Users { get; set; }

    public decimal TotalPrice { get; set; }
}
