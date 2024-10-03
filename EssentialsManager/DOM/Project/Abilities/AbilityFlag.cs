using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Abilities;

public class AbilityFlag
{
    [Key]
    public string FlagName { get; set; }
    public IEnumerable<Ability> Abilities { get; set; }
}