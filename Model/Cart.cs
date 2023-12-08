using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worker.Model;

public class Cart
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    public virtual List<CartItem> CartList { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Users Users { get; set; }
}
