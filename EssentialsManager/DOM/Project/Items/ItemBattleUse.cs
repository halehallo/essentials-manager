using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Items;

public class ItemBattleUse
{
    [Key]
    public string BattleUseName { get; set; }
    public IEnumerable<Item> Items { get; set; }
}