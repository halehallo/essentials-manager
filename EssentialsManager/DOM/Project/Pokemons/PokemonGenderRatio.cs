using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonGenderRatio
{
    [Key]
    public string GenderRatioName { get; set; }
    public IEnumerable<Pokemon> Pokemons { get; set; }
}