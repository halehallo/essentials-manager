using DOM.ProjectFolders;

namespace BL;

public interface IProjectFolderManager
{
    public IEnumerable<Project> GetProjects();
    public Project AddProject(string name, string folderpath);
}