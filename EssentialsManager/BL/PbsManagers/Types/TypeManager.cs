using BL.Exceptions;
using DAL.PbsRepositories.Types;
using DOM.Project.Typings;

namespace BL.PbsManagers.Types;

public class TypeManager : ITypeManager
{
    private readonly ITypingRepository _typingRepository;
    public TypeManager(ITypingRepository typingRepository)
    {
        _typingRepository = typingRepository;
    }

    public void ReadAllTypesFromPbs(Dictionary<string, Dictionary<string, string>> blocks)
    {
        WriteAllTypesWithoutLinksToPbs(blocks);
        LinkAllTypesInDatabase();
    }

    public bool HasData()
    {
        return _typingRepository.HasAnyTyping();
    }

    public IEnumerable<Typing> GetAllTypesWithJoin()
    {
        return _typingRepository.ReadAllTypingsWithJoin();
    }
    public IEnumerable<Typing> GetAllTypesWithFullJoin()
    {
        return _typingRepository.ReadAllTypingsWithFullJoin();
    }

    public void UpdateType(Typing type)
    {
        _typingRepository.UpdateTyping(type);
    }

    private void WriteAllTypesWithoutLinksToPbs(Dictionary<string, Dictionary<string, string>> blocks)
    {
        foreach (var block in blocks)
        {
            block.Value.TryGetValue("Name", out string name);
            
            block.Value.TryGetValue("IconPosition", out string iconPosition);
            iconPosition ??= "0";
            
            block.Value.TryGetValue("IsSpecialType", out string isSpecialType);
            isSpecialType ??= "false";
            
            block.Value.TryGetValue("IsPseudoType", out string isPseudoType);
            isPseudoType ??= "false";
            
            block.Value.TryGetValue("Weaknesses", out string weaknessesString);
            ICollection<string> weaknesses = weaknessesString != null ? weaknessesString.Split(',').ToList() : [];
            
            block.Value.TryGetValue("Resistances", out string resistancesString);
            ICollection<string> resistances = resistancesString != null ? resistancesString.Split(',').ToList() : [];
            
            block.Value.TryGetValue("Immunities", out string immunitiesString);
            ICollection<string> immunities = immunitiesString != null ? immunitiesString.Split(',').ToList() : [];

            Typing typing = new Typing(block.Key, name, int.Parse(iconPosition), bool.Parse(isPseudoType),
                bool.Parse(isSpecialType), weaknesses, resistances, immunities);
            
            _typingRepository.CreateTyping(typing);
            
        }
    }

    private void LinkAllTypesInDatabase()
    {
        IEnumerable<Typing> typings = _typingRepository.ReadAllTypingsWithJoin();
        
        // Create a dictionary to optimize lookups by weakness string
        var typingDictionary = typings.ToDictionary(t => t.InternalName, t => t);

        foreach (var typing in typings)
        {
            foreach (var weaknessString in typing.WeaknessesStrings)
            {
                // Use the dictionary to lookup the typing by its name
                if (typingDictionary.TryGetValue(weaknessString, out var weaknessTyping))
                {
                    TypingWeakness typingWeakness = new TypingWeakness()
                    {
                        KeyString = typing.InternalName + "-" + weaknessString,
                        Typing = typing,
                        Weakness = weaknessTyping
                    };

                    // Add the TypingWeakness to the list instead of saving immediately
                    // typingWeaknessesToCreate.Add(typingWeakness);
                    typing.Weaknesses.Add(typingWeakness);
                }
                else
                {
                    throw new TypingNameMismatchException($"Typing from weaknesses with name '{typing.InternalName}'" +
                                                          $" was not found in database.");
                }
            }
            
            foreach (var resistancesString in typing.ResistancesStrings)
            {
                // Use the dictionary to lookup the typing by its name
                if (typingDictionary.TryGetValue(resistancesString, out var resistanceTyping))
                {
                    TypingResistance typingResistance = new TypingResistance()
                    {
                        KeyString = typing.InternalName + "-" + resistancesString,
                        Typing = typing,
                        Resistance = resistanceTyping
                    };

                    // Add the TypingWeakness to the list instead of saving immediately
                    // typingWeaknessesToCreate.Add(typingWeakness);
                    typing.Resistances.Add(typingResistance);
                }
                else
                {
                    throw new TypingNameMismatchException($"Typing from resistances with name '{typing.InternalName}'" +
                                                          $" was not found in database.");
                }
            }
            
            foreach (var immunitiesString in typing.ImmunitiesStrings)
            {
                // Use the dictionary to lookup the typing by its name
                if (typingDictionary.TryGetValue(immunitiesString, out var immunitiesTyping))
                {
                    TypingImmunities typingImmunity = new TypingImmunities()
                    {
                        KeyString = typing.InternalName + "-" + immunitiesString,
                        Typing = typing,
                        Immunity = immunitiesTyping
                    };

                    // Add the TypingWeakness to the list instead of saving immediately
                    // typingWeaknessesToCreate.Add(typingWeakness);
                    typing.Immunities.Add(typingImmunity);
                }
                else
                {
                    throw new TypingNameMismatchException($"Typing from immunities with name '{typing.InternalName}'" +
                                                          $" was not found in database.");
                }
            }

            // Update the Typing after all its weaknesses have been added
            _typingRepository.UpdateTyping(typing);
        }
        
    }
}