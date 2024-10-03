namespace BL.PbsManagers.Moves;

public interface IMoveManager
{
    void ReadAllMovesFromPbs(Dictionary<string, Dictionary<string, string>> blocks);

}