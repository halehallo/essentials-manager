using DAL.EF;
using DOM.Project.Moves;

namespace DAL.PbsRepositories.Moves;

public class MoveRepository : IMoveRepository
{
    private readonly ProjectDbContext _context;

    public MoveRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public void CreateMove(Move move)
    {
        _context.Moves.Add(move);
        // _context.SaveChanges();
    }

    public Move ReadMoveByInternalName(string name)
    {
        return _context.Moves.Find(name);
    }

    public void CreateCategory(MoveCategory moveCategory)
    {
        _context.MoveCategories.Add(moveCategory);
        // _context.SaveChanges();
    }

    public void CreateMoveFunctionCode(MoveFunctionCode moveFunctionCode)
    {
        _context.MoveFunctionCodes.Add(moveFunctionCode);
        // _context.SaveChanges();
    }

    public void CreateMoveTarget(MoveTarget moveTarget)
    {
        _context.MoveTargets.Add(moveTarget);
        // _context.SaveChanges();
    }

    public void CreateMoveFlag(MoveFlag moveFlag)
    {
        _context.MoveFlags.Add(moveFlag);
        // _context.SaveChanges();
    }

    public void CreateLearnedMove(LearnedMove learnedMove)
    {
        _context.LearnedMoves.Add(learnedMove);
        // _context.SaveChanges();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}