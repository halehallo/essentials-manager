using BL.PbsManagers;
using DAL;
using DOM.ProjectFolders;

namespace BL;

public class ProjectManager : IProjectManager
{
    private readonly IProjectRepository _repository;
    private readonly IPbsManager _pbsManager;
    public string Folderpath { get; set; }
    

    public ProjectManager(IProjectRepository repository, IPbsManager pbsManager)
    {
        _repository = repository;
        _pbsManager = pbsManager;
    }

    public bool ChangeConnectionString(string datasource, string newFolderpath)
    {
        string connectionString = "Data Source=" + datasource;
        Folderpath = newFolderpath;
        _pbsManager.ChangeFolderPath(newFolderpath);
        return _repository.UpdateConnectionString(connectionString);
    }

    public void CompilePbsFiles()
    {
        _pbsManager.LoadAllPbsFiles();
    }
}