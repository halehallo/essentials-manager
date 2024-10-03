namespace BL.PbsManagers.Types;

public interface ITypeManager
{
    void ReadAllTypesFromPbs(Dictionary<string, Dictionary<string, string>> blocks);
}