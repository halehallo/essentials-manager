using System.ComponentModel.DataAnnotations;
using DOM.Project.Typings;

namespace DOM.Project.Moves;

public class Move
{
    [Key]
    public string InternalName { get; set; }
    public string Name { get; set; }
    public Typing Typing { get; set; }
    public MoveCategory Category { get; set; }
    public int Power { get; set; }
    public int Accuracy { get; set; }
    // ReSharper disable once InconsistentNaming
    public int TotalPP { get; set; }
    public MoveTarget Target { get; set; }
    public int Priority { get; set; }
    public MoveFunctionCode FunctionCode { get; set; }
    public IEnumerable<MoveFlag> Flags { get; set; }
    public int EffectChance { get; set; }
    public string Description { get; set; }
}