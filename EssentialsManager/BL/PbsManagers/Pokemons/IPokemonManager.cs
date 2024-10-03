namespace BL.PbsManagers.Pokemons;

public interface IPokemonManager
{
    void ReadAllPokemonFromPbs(Dictionary<string, Dictionary<string, string>> blocks);
}