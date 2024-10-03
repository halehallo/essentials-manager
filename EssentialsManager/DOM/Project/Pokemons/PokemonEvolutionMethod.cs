using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonEvolutionMethod
{
    [Key]
    public string MethodName { get; set; }
    public IEnumerable<PokemonEvolution> Evolutions { get; set; }
}