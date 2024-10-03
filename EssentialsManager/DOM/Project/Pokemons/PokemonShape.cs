using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonShape
{
    [Key]
    public string ShapeName { get; init; }
    public IEnumerable<Pokemon> Pokemons { get; set; }
}