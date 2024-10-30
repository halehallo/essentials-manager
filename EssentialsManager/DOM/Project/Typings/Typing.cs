using System.ComponentModel.DataAnnotations;
using DOM.Project.Pokemons;

namespace DOM.Project.Typings;

public class Typing
{
    [Key]
    public string InternalName { get; set; }
    public string Name { get; set; }
    public int IconPosition { get; set; }
    public bool IsPseudoType { get; set; }
    public bool IsSpecialType { get; set; }
    public ICollection<string> WeaknessesStrings { get; set; }
    public ICollection<TypingWeakness> Weaknesses { get; set; }
    public ICollection<string> ResistancesStrings { get; set; }
    public ICollection<TypingResistance> Resistances { get; set; }
    public ICollection<string> ImmunitiesStrings { get; set; }
    public ICollection<TypingImmunities> Immunities { get; set; }
    
    // navigation
    public IEnumerable<Pokemon> PokemonsWithTyping { get; set; }

    public Typing(string internalName, string name, int iconPosition, bool isPseudoType, bool isSpecialType, ICollection<string> weaknessesStrings, ICollection<string> resistancesStrings, ICollection<string> immunitiesStrings)
    {
        InternalName = internalName;
        Name = name;
        IconPosition = iconPosition;
        IsPseudoType = isPseudoType;
        IsSpecialType = isSpecialType;
        WeaknessesStrings = weaknessesStrings;
        Weaknesses = new List<TypingWeakness>();
        ResistancesStrings = resistancesStrings;
        Resistances = new List<TypingResistance>();
        ImmunitiesStrings = immunitiesStrings;
        Immunities = new List<TypingImmunities>();
    }

    public override string ToString()
    {
        return string.Format("Typing: {{ InternalName: {0}, Name: {1}, IconPosition: {2}, IsPseudoType: {3}, IsSpecialType: {4}, Weaknesses: [{5}], Resistances: [{6}], Immunities: [{7}] }}",
            InternalName,
            Name,
            IconPosition,
            IsPseudoType,
            IsSpecialType,
            string.Join(", ", WeaknessesStrings),
            string.Join(", ", ResistancesStrings),
            string.Join(", ", ImmunitiesStrings));
    }
}