namespace BL.PbsManagers.Items;

public interface IItemManager
{
    void ReadAllItemsFromPbs(Dictionary<string, Dictionary<string, string>> blocks);

}