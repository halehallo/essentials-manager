using DOM.Project.Typings;

namespace BL.PbsManagers.Types;

public interface ITypeManager
{
    void ReadAllTypesFromPbs(Dictionary<string, Dictionary<string, string>> blocks);
    bool HasData();
    IEnumerable<Typing> GetAllTypesWithJoin();
    IEnumerable<Typing> GetAllTypesWithFullJoin();
    int getAmountOfTypings();
    void UpdateType(Typing type);
}