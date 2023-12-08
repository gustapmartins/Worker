using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worker.Model;

public  class Category
{
    [Key]
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
