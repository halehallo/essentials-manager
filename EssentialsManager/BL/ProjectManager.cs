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

    public bool ChangeConnectionString(string newFolderpath)
    {
        string connectionString = "Data Source=" + newFolderpath+ "\\EssentialsManager\\project.db";
        Folderpath = newFolderpath;
        _pbsManager.ChangeFolderPath(newFolderpath);
        return _repository.UpdateConnectionString(connectionString);
    }

    public bool ResetConnectionString()
    {
        string connectionString = "Data Source=defaultProjectDatabase.db";
        Folderpath = "";
        _pbsManager.ChangeFolderPath("");
        return _repository.UpdateConnectionString(connectionString);
    }

    public void CompilePbsFiles()
    {
        _pbsManager.LoadAllPbsFiles();
    }
}