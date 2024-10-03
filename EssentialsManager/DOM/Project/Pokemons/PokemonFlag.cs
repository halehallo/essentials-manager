using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonFlag
{
    [Key]
    public string FlagName { get; set; }
    public IEnumerable<Pokemon> Pokemons { get; set; }
}