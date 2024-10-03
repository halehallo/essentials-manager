using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonEvolution
{
    [Key]
    public int Id { get; set; }
    public Pokemon PokemonBefore { get; set; }
    public Pokemon PokemonAfter { get; set; }
    public string PokemonBeforeString { get; set; }
    public string PokemonAfterString { get; set; }
    public PokemonEvolutionMethod PokemonEvolutionMethod { get; set; }
    public string Parameter { get; set; }
}