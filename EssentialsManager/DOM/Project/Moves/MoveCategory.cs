using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Moves;

public class MoveCategory
{
    [Key]
    public string CategoryName { get; set; }
    public IEnumerable<Move> Moves { get; set; }
}