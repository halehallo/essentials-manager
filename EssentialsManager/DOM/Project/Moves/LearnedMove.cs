using System.ComponentModel.DataAnnotations;
using DOM.Project.Pokemons;

namespace DOM.Project.Moves;

public class LearnedMove
{
    [Key]
    public int Id { get; set; }
    public Move Move { get; set; }
    public int Level { get; set; }
    public Pokemon Pokemon { get; set; }
}