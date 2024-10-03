using DOM.Project.Moves;

namespace DAL.PbsRepositories.Moves;

public interface IMoveRepository
{
    void CreateMove(Move move);
    Move ReadMoveByInternalName(string name);
    void CreateCategory(MoveCategory moveCategory);
    void CreateMoveFunctionCode(MoveFunctionCode moveFunctionCode);
    void CreateMoveTarget(MoveTarget moveTarget);
    void CreateMoveFlag(MoveFlag moveFlag);
    void CreateLearnedMove(LearnedMove learnedMove);
    void SaveChanges();
    
}