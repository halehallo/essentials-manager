using BL.DataTransferObjects;
using BL.PbsManagers.Abilities;
using BL.PbsManagers.Items;
using BL.PbsManagers.Moves;
using BL.PbsManagers.Pokemons;
using BL.PbsManagers.Types;
using DAL;
using DOM.Project.Pokemons;
using DOM.Project.Typings;

namespace BL.PbsManagers;

public class PbsManager : IPbsManager
{
    private readonly IProjectRepository _projectRepository;
    private ITypeManager _typeManager;
    private IAbilityManager _abilityManager;
    private IMoveManager _moveManager;
    private IItemManager _itemManager;
    private IPokemonManager _pokemonManager;
    private string FolderPath { get; set; }

    public PbsManager(IProjectRepository projectRepository, ITypeManager typeManager,
        IAbilityManager abilityManager, IMoveManager moveManager, IItemManager itemManager,
        IPokemonManager pokemonManager)
    {
        _projectRepository = projectRepository;
        _typeManager = typeManager;
        _abilityManager = abilityManager;
        _moveManager = moveManager;
        _itemManager = itemManager;
        _pokemonManager = pokemonManager;
    }


    public void ChangeFolderPath(string folderpath)
    {
        FolderPath = folderpath;
    }

    private Dictionary<string, Dictionary<string, string>> ReadBlocks(string filePath)
    {
        // Dictionary to store the properties of each block
        Dictionary<string, Dictionary<string, string>> blocks = new Dictionary<string, Dictionary<string, string>>();

        // Read the text file line by line
        foreach (string line in File.ReadAllLines(filePath))
        {
            // Ignore lines starting with '#'
            if (!line.StartsWith('#'))
            {
                // Check if the line starts with '[' indicating the start of a new block
                if (line.StartsWith('['))
                {
                    // split comment from internal name
                    string[] parts = line.Split('#');
                    
                    // Remove leading/trailing whitespace from internal name
                    string internalName = parts[0].Trim();
                    
                    // Extract the block name
                    string blockName = internalName.Trim('[', ']');

                    // Initialize a new dictionary to store properties of this block
                    Dictionary<string, string> properties = new Dictionary<string, string>();

                    // Add the block to the dictionary
                    blocks.Add(blockName, properties);
                    
                    // if there is a comment, add it to the same block
                    if (parts.Length > 1)
                    {
                        // Get the last block added to the dictionary
                        var lastBlock = blocks.LastOrDefault();
                        
                        // trim the comment of leading or trailing spaces
                        string comment = parts[1].Trim();
                        
                        // Add the property to the last block
                        if (lastBlock.Key != null)
                        {
                            lastBlock.Value.Add("Comment", comment);
                        }
                    }
                }
                else
                {
                    // Split the line by '=' to extract property name and value
                    string[] parts = line.Split('=');

                    // Remove leading/trailing whitespace from property name and value
                    string propertyName = parts[0].Trim();
                    string propertyValue = parts[1].Trim();

                    // Get the last block added to the dictionary
                    var lastBlock = blocks.LastOrDefault();

                    // Add the property to the last block
                    if (lastBlock.Key != null)
                    {
                        lastBlock.Value.Add(propertyName, propertyValue);
                    }
                }
            }
        }

        return blocks;
    }

    public void LoadAllPbsFiles()
    {
        LoadTypes();
        LoadAbilities();
        LoadMoves();
        LoadItems();
        LoadPokemon();
    }

    public bool HasDataSaved()
    {
        return _typeManager.HasData();
    }

    public void SaveTypeEffectivenessChanges(ICollection<TypeEffectivenessFieldChange> changes)
    {
        List<Typing> typings = _typeManager.GetAllTypesWithFullJoin().OrderBy(t => t.IconPosition).ToList();
        foreach (TypeEffectivenessFieldChange change in changes)
        {
            Typing attackingType = typings[change.AttackingType];
            Typing defendingType = typings[change.DefendingType];

            if (change.InitialState.Equals("Weakness"))
            {
                defendingType.Weaknesses = defendingType.Weaknesses.Where(weakness => !weakness.Weakness.Equals(attackingType)).ToList();
            }else if (change.InitialState.Equals("Resistance"))
            {
                defendingType.Resistances = defendingType.Resistances.Where(resistance => !resistance.Resistance.Equals(attackingType)).ToList();
            }else if (change.InitialState.Equals("Immunity"))
            {
                defendingType.Immunities = defendingType.Immunities.Where(immunity => !immunity.Immunity.Equals(attackingType)).ToList();
            }
            
            if (change.State.Equals("Weakness"))
            {
                defendingType.Weaknesses.Add(new TypingWeakness()
                {
                    Weakness = attackingType,
                    Typing = defendingType,
                    KeyString = $"{defendingType.InternalName}-{attackingType.InternalName}"
                });
            }else if (change.State.Equals("Resistance"))
            {
                defendingType.Resistances.Add(new TypingResistance()
                {
                    Resistance = attackingType,
                    Typing = defendingType,
                    KeyString = $"{defendingType.InternalName}-{attackingType.InternalName}"
                });
            }else if (change.State.Equals("Immunity"))
            {
                defendingType.Immunities.Add(new TypingImmunities()
                {
                    Immunity = attackingType,
                    Typing = defendingType,
                    KeyString = $"{defendingType.InternalName}-{attackingType.InternalName}"
                });
            }
            _typeManager.UpdateType(defendingType);
        }
    }

    public void SavePokemonAvailabilityChanges(ICollection<PokemonAvailabilityChange> changes)
    {
        foreach (PokemonAvailabilityChange change in changes)
        {
            Pokemon pokemonToChange = _pokemonManager.GetPokemonFromKeyName(change.KeyName);
            pokemonToChange.IsEvent = change.IsEventPokemon;
            pokemonToChange.IsGift = change.IsGift;
            _pokemonManager.UpdatePokemon(pokemonToChange);
        }
        _pokemonManager.SaveChanges();
    }

    public void SaveTypingsToPbsFromDatabase()
    {
        string filePath = FolderPath + "\\PBS\\types_test.txt";
        List<Typing> typings = _typeManager.GetAllTypesWithFullJoin().OrderBy(t => t.IconPosition).ToList();
        
        List<string> lines = new List<string>();
        
        lines.Add("# See the documentation on the wiki to learn how to edit this file.");
        lines.Add("#------------------------------- ");
        
        foreach (Typing typing in typings)
        {
            lines.Add($"[{typing.InternalName}]");
            lines.Add($"Name = {typing.Name}");
            lines.Add($"IconPosition = {typing.IconPosition}");
            
            if (typing.IsSpecialType)
                lines.Add("IsSpecialType = true");
            
            if (typing.IsPseudoType)
                lines.Add("IsPseudoType = true");
            
            // if necessary, add flags
            // have not seen it used though
            
            if (typing.Weaknesses.Count > 0)
                lines.Add($"Weaknesses = {string.Join(",", typing.Weaknesses.Select(w => w.Weakness.InternalName))}");

            if (typing.Resistances.Count > 0)
                lines.Add($"Resistances = {string.Join(",", typing.Resistances.Select(r => r.Resistance.InternalName))}");

            if (typing.Immunities.Count > 0)
                lines.Add($"Immunities = {string.Join(",", typing.Immunities.Select(i => i.Immunity.InternalName))}");

            lines.Add("#-------------------------------");
        }

        File.WriteAllLines(filePath, lines);
    }

    private void LoadTypes()
    {
        // Path to your text file
        string filePath = FolderPath + "\\PBS\\types.txt";

        Dictionary<string, Dictionary<string, string>> blocks = ReadBlocks(filePath);

        // Output the parsed blocks and their properties
        _typeManager.ReadAllTypesFromPbs(blocks);
    }
    
    private void LoadAbilities()
    {
        string filePath = FolderPath + "\\PBS\\abilities.txt";

        Dictionary<string, Dictionary<string, string>> blocks = ReadBlocks(filePath);
        
        _abilityManager.ReadAllAbilitiesFromPbs(blocks);
    }
    
    private void LoadMoves()
    {
        string filePath = FolderPath + "\\PBS\\moves.txt";

        Dictionary<string, Dictionary<string, string>> blocks = ReadBlocks(filePath);
        
        _moveManager.ReadAllMovesFromPbs(blocks);
    }
    
    private void LoadItems()
    {
        string filePath = FolderPath + "\\PBS\\items.txt";

        Dictionary<string, Dictionary<string, string>> blocks = ReadBlocks(filePath);
        
        _itemManager.ReadAllItemsFromPbs(blocks);
    }
    
    private void LoadPokemon()
    {
        string filePathBase = FolderPath + "\\PBS\\pokemon.txt";

        Dictionary<string, Dictionary<string, string>> blocksBase = ReadBlocks(filePathBase);
        
        string filePathForms = FolderPath + "\\PBS\\pokemon_forms.txt";

        Dictionary<string, Dictionary<string, string>> blocksForms = ReadBlocks(filePathForms);
        
        _pokemonManager.WriteAllPokemonWithoutLinksToPbs(blocksBase);
        _pokemonManager.WriteAllPokemonWithoutLinksToPbs(blocksForms);
        _pokemonManager.LinkAllPokemonInDatabase();
    }
    
   
}