using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Pokemons;

public class PokemonEvGained
{
    [Key]
    public string EvGainedName { get; set; }
    public int EvValue {get; set;}
    public EvType EvType {get; set;}
    
    public IEnumerable<Pokemon> Pokemons { get; set; }
}