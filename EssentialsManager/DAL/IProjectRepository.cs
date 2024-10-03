using DOM.ProjectFolders;

namespace DAL;

public interface IProjectRepository
{
    public bool UpdateConnectionString(string connectionString);
}