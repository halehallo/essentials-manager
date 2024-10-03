using System.ComponentModel.DataAnnotations;

namespace DOM.Project.Typings;

public class TypingWeakness
{
    [Key]
    public string KeyString { get; set; }
    public Typing Typing { get; set; }
    public Typing Weakness { get; set; }
}