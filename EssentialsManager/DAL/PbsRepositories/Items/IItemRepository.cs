using DOM.Project.Items;

namespace DAL.PbsRepositories.Items;

public interface IItemRepository
{
    void CreateItem(Item item);
    Item ReadItemByItemName(string itemName);
    void CreateItemBattleUse(ItemBattleUse itemBattleUse);
    void CreateItemFieldUse(ItemFieldUse itemFieldUse);
    void CreateItemFlag(ItemFlag itemFlag);
    void SaveChanges();
}