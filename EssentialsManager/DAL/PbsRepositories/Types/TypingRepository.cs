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

    public ICollection<Typing> ReadAllTypingsWithJoin()
    {
        return _context.Types
            .Include(t => t.Weaknesses)
            .Include(t => t.Resistances)
            .Include(t => t.Immunities)
            .ToList();
    }

    public void CreateTypingWeakness(TypingWeakness weakness)
    {
        _context.TypeWeaknesses.Add(weakness);
        _context.SaveChanges();
    }
    public void CreateTypingWeaknessesBatch(IEnumerable<TypingWeakness> typingWeaknesses)
    {
        _context.TypeWeaknesses.AddRange(typingWeaknesses);
        _context.SaveChanges();
    }
}