using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Typings;

public class TypingResistance
{
    [Key]
    public string KeyString { get; set; }
    public Typing Typing { get; set; }
    
    public Typing Resistance { get; set; }
}