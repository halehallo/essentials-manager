using DAL.PbsRepositories.Abilities;
using DOM.Project.Abilities;

namespace BL.PbsManagers.Abilities;

public class AbilityManager : IAbilityManager
{
    private readonly IAbilityRepository _abilityRepository;

    public AbilityManager(IAbilityRepository abilityRepository)
    {
        _abilityRepository = abilityRepository;
    }

    public void ReadAllAbilitiesFromPbs(Dictionary<string, Dictionary<string, string>> blocks)
    {
        Dictionary<string, AbilityFlag> abilityFlagsDictionary = new Dictionary<string, AbilityFlag>(1);
        foreach (var block in blocks)
        {
            block.Value.TryGetValue("Name", out string name);

            block.Value.TryGetValue("Description", out string description);
            
            block.Value.TryGetValue("Flags", out string flagsString);
            ICollection<string> flags = flagsString != null ? flagsString.Split(',').ToList() : [];
            List<AbilityFlag> abilityFlags = [];

            foreach (string flag in flags)
            {
                abilityFlagsDictionary.TryGetValue(flag, out AbilityFlag abilityFlag);
                if (abilityFlag == null)
                {
                    abilityFlag = new AbilityFlag()
                    {
                        FlagName = flag,
                    };
                    _abilityRepository.CreateAbilityFlag(abilityFlag);
                    abilityFlagsDictionary.Add(flag, abilityFlag);
                }
                abilityFlags.Add(abilityFlag);
            }

            Ability ability = new Ability()
            {
                Name = name,
                InternalName = block.Key,
                Description = description,
                Flags = abilityFlags
            };
            
            _abilityRepository.CreateAbility(ability);
            
        }
    }
}