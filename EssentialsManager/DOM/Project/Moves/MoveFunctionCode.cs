using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Moves;

public class MoveFunctionCode
{
    [Key]
    public string FunctionName { get; init; }
    public IEnumerable<Move> Moves { get; set; }

}