using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worker.Model;

public class Show
{
    [Key]
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }

    public string Local { get; set; }

    public virtual Category? Category { get; set; }
}
