using DAL.EF;
using DOM.Project.Abilities;

namespace DAL.PbsRepositories.Abilities;

public class AbilityRepository : IAbilityRepository
{
    private readonly ProjectDbContext _context;

    public AbilityRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public void CreateAbility(Ability ability)
    {
        _context.Abilities.Add(ability);
        _context.SaveChanges();
    }

    public Ability ReadAbilityByAbilityName(string abilityName)
    {
        return _context.Abilities.Find(abilityName);
    }

    public void CreateAbilityFlag(AbilityFlag abilityFlag)
    {
        _context.AbilityFlags.Add(abilityFlag);
        // _context.SaveChanges();
    }

    public AbilityFlag ReadAbilityFlagByName(string abilityFlagName)
    {
        return _context.AbilityFlags.Find(abilityFlagName);
    }
}