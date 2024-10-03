using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonColor
{
    [Key]
    public string ColorName { get; set; }
    public IEnumerable<Pokemon> Pokemons { get; set; }
}