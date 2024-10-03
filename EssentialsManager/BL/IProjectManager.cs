using DOM.ProjectFolders;

namespace BL;

public interface IProjectManager
{
    public bool ChangeConnectionString(string connectionString, string folderpath);
    public void CompilePbsFiles();
}