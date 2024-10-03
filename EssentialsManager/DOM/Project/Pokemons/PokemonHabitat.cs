using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonHabitat
{
    [Key]
    public string HabitatName { get; set; }
    public IEnumerable<Pokemon> Pokemons { get; set; }
}