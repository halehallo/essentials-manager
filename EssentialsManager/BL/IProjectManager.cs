using BL.DataTransferObjects;
using DOM.Project.Typings;
using DOM.ProjectFolders;

namespace BL;

public interface IProjectManager
{
    public bool ChangeConnectionString(string folderpath);
    public bool ResetConnectionString();
    public void CompilePbsFiles();
    public IEnumerable<Typing> GetAllTypingsWithFullJoins();
    public void ChangeTypeEffectiveness(ICollection<TypeEffectivenessFieldChange> changes);
    public string GetProjectFolderPath();
}