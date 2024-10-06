using BL.PbsManagers.Abilities;
using BL.PbsManagers.Items;
using BL.PbsManagers.Moves;
using BL.PbsManagers.Pokemons;
using BL.PbsManagers.Types;
using DAL;

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
        string filePath = FolderPath + "\\PBS\\pokemon.txt";

        Dictionary<string, Dictionary<string, string>> blocks = ReadBlocks(filePath);
        
        _pokemonManager.ReadAllPokemonFromPbs(blocks);
    }
}