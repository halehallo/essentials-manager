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
    }

    public Move ReadMoveByInternalName(string name)
    {
        return _context.Moves.Find(name);
    }

    public void CreateCategory(MoveCategory moveCategory)
    {
        _context.MoveCategories.Add(moveCategory);
    }

    public void CreateMoveFunctionCode(MoveFunctionCode moveFunctionCode)
    {
        _context.MoveFunctionCodes.Add(moveFunctionCode);
    }

    public void CreateMoveTarget(MoveTarget moveTarget)
    {
        _context.MoveTargets.Add(moveTarget);
    }

    public void CreateMoveFlag(MoveFlag moveFlag)
    {
        _context.MoveFlags.Add(moveFlag);
    }

    public void CreateLearnedMove(LearnedMove learnedMove)
    {
        _context.LearnedMoves.Add(learnedMove);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
    }
}