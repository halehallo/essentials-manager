using System.ComponentModel.DataAnnotations;
using DOM.Project.Abilities;
using DOM.Project.Items;
using DOM.Project.Moves;
using DOM.Project.Typings;

namespace DOM.Project.Pokemons;

public class Pokemon
{
    [Key]
    public string KeyName {get; set;}
    public string InternalName { get; set; }
    public int FormNumber { get; set; }
    public int DexNumber {get; set;}
    public string Name { get; set; }
    public string FormName { get; set; }
    public IEnumerable<Typing> Typings { get; set; }
    public int Hp { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefense { get; set; }
    public PokemonGenderRatio GenderRatio { get; set; }
    public PokemonGrowthRate GrowthRate { get; set; }
    public int BaseExp { get; set; }
    public IEnumerable<PokemonEvGained> EvsGained { get; set; }
    public int CatchRate {get; set;}
    public int Happiness {get; set;}
    public IEnumerable<Ability> Abilities { get; set; }
    public IEnumerable<Ability> HiddenAbilities { get; set; }
    public IEnumerable<LearnedMove> Moves { get; set; }
    public IEnumerable<Move> TutorMoves { get; set; }
    public IEnumerable<Move> EggMoves { get; set; }
    public IEnumerable<PokemonEggGroup> EggGroups { get; set; }
    public int HatchSteps { get; set; }
    public Item Incense { get; set; }
    public IList<Pokemon> Offspring { get; set; }
    public IList<string> OffspringStrings { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public PokemonColor Color { get; set; }
    public PokemonShape Shape { get; set; }
    public PokemonHabitat Habitat { get; set; }
    public string Category { get; set; }
    public string Pokedex { get; set; }
    public int Generation { get; set; }
    public IEnumerable<PokemonFlag> Flags { get; set; }
    public Item WildItemCommon { get; set; }
    public Item WildItemUncommon { get; set; }
    public Item WildItemRare  { get; set; }
    public ICollection<PokemonEvolution> Evolutions { get; set; }
    public Item MegaStone { get; set; }
    public int UnmegaForm { get; set; }
    public Move MegaMove {get; set;}
    public int MegaMessage { get; set; }
    public bool IsCatchable { get; set; }
    public bool IsEvent { get; set; }
    public bool IsGift { get; set; }

    //navigation prop
    public ICollection<Pokemon> Parents { get; set; }
}