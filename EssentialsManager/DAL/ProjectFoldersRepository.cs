using DAL.EF;
using DOM.ProjectFolders;

namespace DAL;

public class ProjectFoldersRepository : IProjectFoldersRepository
{
    private readonly ProjectFoldersDbContext _context;

    public ProjectFoldersRepository(ProjectFoldersDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Project> ReadAllProjects()
    {
        return _context.Projects.AsEnumerable();
    }

    public Project ReadProjectByNameAndFolderPath(string name, string folderpath)
    {
        return _context.Projects
            .FirstOrDefault(project => project.Name.Equals(name) && project.FolderPath.Equals(folderpath));
    }

    public Project CreateProject(Project project)
    {
        _context.Projects.Add(project);
        _context.SaveChanges();
        return project;
    }
}