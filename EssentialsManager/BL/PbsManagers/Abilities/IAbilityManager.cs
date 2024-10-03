namespace BL.PbsManagers.Abilities;

public interface IAbilityManager
{
    void ReadAllAbilitiesFromPbs(Dictionary<string, Dictionary<string, string>> blocks);

}