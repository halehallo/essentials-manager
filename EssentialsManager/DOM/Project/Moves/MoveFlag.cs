using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Moves;

public class MoveFlag
{
    [Key]
    public string FlagName { get; set; }
    public IEnumerable<Move> Moves { get; set; }

}