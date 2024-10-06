using DOM.ProjectFolders;

namespace BL;

public interface IProjectManager
{
    public bool ChangeConnectionString(string folderpath);
    public bool ResetConnectionString();
    public void CompilePbsFiles();
}