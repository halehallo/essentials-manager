using DAL.EF;
using DOM.Project.Typings;
using Microsoft.EntityFrameworkCore;

namespace DAL.PbsRepositories.Types;

public class TypingRepository : ITypingRepository
{
    private readonly ProjectDbContext _context;
    
    public TypingRepository(ProjectDbContext context)
    {
        _context = context;
    }


    public void CreateTyping(Typing type)
    {
        _context.Types.Add(type);
        _context.SaveChanges();
    }

    public Typing ReadTyping(string internalName)
    {
        return _context.Types.FirstOrDefault(t => t.InternalName == internalName);
    }

    public void UpdateTyping(Typing type)
    {
        _context.Types.Update(type);
        _context.SaveChanges();
    }

    public ICollection<Typing> ReadAllTypings()
    {
        return _context.Types
            .ToList();
    }

    public IEnumerable<Typing> ReadAllTypingsWithJoin()
    {
        return _context.Types
            .Include(t => t.Weaknesses)
            .ThenInclude(w => w.Weakness)
            .Include(t => t.Resistances)
            .ThenInclude(r => r.Resistance)
            .Include(t => t.Immunities)
            .ThenInclude(i => i.Immunity)
            .ToList();
    }
    public IEnumerable<Typing> ReadAllTypingsWithFullJoin()
    {
        return _context.Types
            .Include(t => t.Weaknesses)
            .ThenInclude(w => w.Weakness)
            .Include(t => t.Resistances)
            .ThenInclude(r => r.Resistance)
            .Include(t => t.Immunities)
            .ThenInclude(i => i.Immunity)
            .ToList();
    }

    public bool HasAnyTyping()
    {
        return _context.Types.Any();
    }

    
}