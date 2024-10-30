using DAL.EF;
using DOM.Project.Items;

namespace DAL.PbsRepositories.Items;

public class ItemRepository : IItemRepository
{
    private readonly ProjectDbContext _context;

    public ItemRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public void CreateItem(Item item)
    {
        _context.Items.Add(item);
    }

    public Item ReadItemByItemName(string itemName)
    {
        return _context.Items.Find(itemName);
    }

    public void CreateItemBattleUse(ItemBattleUse itemBattleUse)
    {
        _context.ItemBattleUses.Add(itemBattleUse);
    }

    public void CreateItemFieldUse(ItemFieldUse itemFieldUse)
    {
        _context.ItemFieldUses.Add(itemFieldUse);
    }

    public void CreateItemFlag(ItemFlag itemFlag)
    {
        _context.ItemFlags.Add(itemFlag);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
    }
}