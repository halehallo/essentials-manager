using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Items;

public class ItemFlag
{
    [Key]
    public string FlagName { get; set; }
    public IEnumerable<Item> Items { get; set; }
}