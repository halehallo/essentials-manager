using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Items;

public class ItemFieldUse
{
    [Key]
    public string FieldUseName { get; set; }
    public IEnumerable<Item> Items { get; set; }
}