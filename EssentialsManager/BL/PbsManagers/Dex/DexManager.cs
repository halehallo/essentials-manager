using BL.DataTransferObjects;
using DAL.PbsRepositories.Pokemons;

namespace BL.PbsManagers.Dex;

public class DexManager : IDexManager
{
    private readonly IPokemonRepository _pokemonRepository;

    public DexManager(IPokemonRepository pokemonRepository)
    {
        _pokemonRepository = pokemonRepository;
    }

    public IEnumerable<DexTypeCountObject> GetAllTypeCounts()
    {
        var allPokemonsWithTypings = _pokemonRepository.ReadAllPokemonsWithTypings();
        
        return allPokemonsWithTypings
            .Where(pokemon => pokemon.IsCatchable || pokemon.IsEvent || pokemon.IsGift) // Filter based on boolean conditions
            .SelectMany(pokemon => 
                    pokemon.Typings.Count > 1 
                        ? new[] { pokemon.Typings[0], pokemon.Typings[1] } 
                        : new[] { pokemon.Typings[0] } // Include only the first typing if there's no second one
            )
            .Where(to => to != null) // Exclude null references
            .GroupBy(to => to) // Group by each unique typingObject
            .Select(group => new DexTypeCountObject
            {
                Type = group.Key.Name,
                Count = group.Count()
            })
            .OrderByDescending(x => x.Count)
            .ToList();
    }
}