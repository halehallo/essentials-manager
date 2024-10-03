using DOM.ProjectFolders;

namespace DAL;

public interface IProjectFoldersRepository
{
    public IEnumerable<Project> ReadAllProjects();
    public Project ReadProjectByNameAndFolderPath(string name, string folderpath);
    public Project CreateProject(Project project);
}