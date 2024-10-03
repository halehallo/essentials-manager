using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonGrowthRate
{
    [Key]
    public string GrowthRateName { get; set; }
    public IEnumerable<Pokemon> Pokemons { get; set; }
}