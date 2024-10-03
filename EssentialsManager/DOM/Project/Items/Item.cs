using System.ComponentModel.DataAnnotations;
using DOM.Project.Moves;

namespace DOM.Project.Items;

public class Item
{
    [Key]
    public string InternalName { get; set; }
    public string Name { get; set; }
    public string NamePlural { get; set; }
    public string PortionName { get; set; }
    public string PortionNamePlural { get; set; }
    public int Pocket { get; set; }
    public int Price { get; set; }
    public int SellPrice { get; set; }
    // ReSharper disable once InconsistentNaming
    public int BPPrice { get; set; }
    public ItemBattleUse BattleUse { get; set; }
    public ItemFieldUse FieldUse { get; set; }
    public IEnumerable<ItemFlag> Flags { get; set; }
    public bool Consumable { get; set; }
    public bool ShowQuantity { get; set; }
    public Move Move { get; set; }
    public string Description { get; set; }
}