using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Typings;

public class TypingImmunities
{
    [Key]
    public string KeyString { get; set; }
    public Typing Typing { get; set; }
    
    public Typing Immunity { get; set; }
}