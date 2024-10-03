using DOM.Project.Abilities;

namespace DAL.PbsRepositories.Abilities;

public interface IAbilityRepository
{
    void CreateAbility(Ability ability);
    Ability ReadAbilityByAbilityName(string abilityName);
    void CreateAbilityFlag(AbilityFlag abilityFlag);
    AbilityFlag ReadAbilityFlagByName(string abilityFlagName);
}