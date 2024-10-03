using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Moves;

public class MoveTarget
{
    [Key]
    public string TargetName { get; set; }
    public IEnumerable<Move> Moves { get; set; }

}