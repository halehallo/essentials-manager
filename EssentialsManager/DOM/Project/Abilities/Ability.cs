using System.ComponentModel.DataAnnotations;
using DOM.Project.Pokemons;

namespace DOM.Project.Abilities;

public class Ability
{
    [Key]
    public string InternalName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<AbilityFlag> Flags { get; set; }
    
    public IEnumerable<Pokemon> PokemonsWithAbility { get; set; }
    public IEnumerable<Pokemon> PokemonsWithHiddenAbility { get; set; }

}