using DAL.PbsRepositories.Items;
using DAL.PbsRepositories.Moves;
using DOM.Project.Items;
using DOM.Project.Moves;

namespace BL.PbsManagers.Items;

public class ItemManager : IItemManager
{
    private readonly IMoveRepository _moveRepository;
    private readonly IItemRepository _itemRepository;

    public ItemManager(IMoveRepository moveRepository, IItemRepository itemRepository)
    {
        _moveRepository = moveRepository;
        _itemRepository = itemRepository;
    }

    public void ReadAllItemsFromPbs(Dictionary<string, Dictionary<string, string>> blocks)
    {
        Dictionary<string, ItemFieldUse> itemFieldUseDictionary = new Dictionary<string, ItemFieldUse>(5);
        Dictionary<string, ItemBattleUse> itemBattleUseDictionary = new Dictionary<string, ItemBattleUse>(6);
        Dictionary<string, ItemFlag> itemFlagDictionary = new Dictionary<string, ItemFlag>(82);

        foreach (var block in blocks)
        {
            block.Value.TryGetValue("Name", out string name);
            name ??= "Unnamed";
            
            block.Value.TryGetValue("NamePlural", out string namePlural);
            namePlural ??= "Unnamed";
            
            block.Value.TryGetValue("PortionName", out string portionName);
            portionName ??= "none";
            
            block.Value.TryGetValue("PortionNamePlural", out string portionNamePlural);
            portionNamePlural ??= "none";
            
            block.Value.TryGetValue("Pocket", out string pocket);
            pocket ??= "1";
            
            block.Value.TryGetValue("Price", out string price);
            price ??= "0";
            
            block.Value.TryGetValue("SellPrice", out string sellPrice);
            sellPrice ??= "0";
            
            block.Value.TryGetValue("BPPrice", out string bPPrice);
            bPPrice ??= "1";
            
            block.Value.TryGetValue("FieldUse", out string fieldUseString);
            fieldUseString ??= "none";
            
            itemFieldUseDictionary.TryGetValue(fieldUseString, out ItemFieldUse fieldUse);
            if (fieldUse == null)
            {
                fieldUse = new ItemFieldUse()
                {
                    FieldUseName = fieldUseString,
                };
                _itemRepository.CreateItemFieldUse(fieldUse);
                itemFieldUseDictionary.Add(fieldUseString, fieldUse);
            }
            
            block.Value.TryGetValue("BattleUse", out string battleUseString);
            battleUseString ??= "none";
            
            itemBattleUseDictionary.TryGetValue(battleUseString, out ItemBattleUse battleUse);
            if (battleUse == null)
            {
                battleUse = new ItemBattleUse()
                {
                    BattleUseName = battleUseString,
                };
                _itemRepository.CreateItemBattleUse(battleUse);
                itemBattleUseDictionary.Add(battleUseString, battleUse);
            }
            
            block.Value.TryGetValue("Flags", out string itemFlagsString);
            IEnumerable<string> flagsStrings = itemFlagsString != null ? itemFlagsString.Split(',').ToList() : [];
            List<ItemFlag> itemFlags = [];

            foreach (string flag in flagsStrings)
            {
                itemFlagDictionary.TryGetValue(flag, out ItemFlag itemFlag);
                if (itemFlag == null)
                {
                    itemFlag = new ItemFlag()
                    {
                        FlagName = flag,
                    };
                    _itemRepository.CreateItemFlag(itemFlag);
                    itemFlagDictionary.Add(flag, itemFlag);
                }
                itemFlags.Add(itemFlag);
            }
            
            block.Value.TryGetValue("Consumable", out string consumable);
            consumable ??=  pocket.Equals("8") ? "false" : "true";
            
            block.Value.TryGetValue("ShowQuantity", out string showQuantity);
            showQuantity ??=  pocket.Equals("8") ? "false" : "true";
            
            block.Value.TryGetValue("Move", out string moveString);
            Move move = null;
            if (moveString != null)
            {
                move = _moveRepository.ReadMoveByInternalName(moveString);
            }
            
            block.Value.TryGetValue("Description", out string description);
            description ??= "???";

            Item item = new Item()
            {
                InternalName = block.Key,
                Name = name,
                NamePlural = namePlural,
                PortionName = portionName,
                PortionNamePlural = portionNamePlural,
                Pocket = int.Parse(pocket),
                Price = int.Parse(price),
                SellPrice = int.Parse(sellPrice),
                BPPrice = int.Parse(bPPrice),
                BattleUse = battleUse,
                FieldUse = fieldUse,
                Flags = itemFlags,
                Consumable = bool.Parse(consumable),
                ShowQuantity = bool.Parse(showQuantity),
                Move = move,
                Description = description,
            };
            
            _itemRepository.CreateItem(item);

        }
        _itemRepository.SaveChanges();
    }
}