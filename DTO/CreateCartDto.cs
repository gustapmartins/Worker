using System.ComponentModel.DataAnnotations;

namespace Worker.DTO;

public class CreateCartDto
{
    [Required]
    public string TicketId { get; set; }

    [Required]
    public int Quantity { get; set; }
}
