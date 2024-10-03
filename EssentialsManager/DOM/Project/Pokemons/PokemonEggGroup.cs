using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonEggGroup
{
    [Key]
    public string EggGroupName { get; set; }
    public IEnumerable<Pokemon> Pokemons { get; set; }
}