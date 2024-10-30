using DOM.Project.Pokemons;

namespace BL.PbsManagers.Pokemons;

public interface IPokemonManager
{
    void WriteAllPokemonWithoutLinksToPbs(Dictionary<string, Dictionary<string, string>> blocks);
    void LinkAllPokemonInDatabase();
    IEnumerable<Pokemon> GetAllPokemonWithTypings();
}