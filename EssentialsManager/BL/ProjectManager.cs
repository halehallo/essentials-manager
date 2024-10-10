using BL.DataTransferObjects;
using BL.PbsManagers;
using BL.PbsManagers.Types;
using DAL;
using DOM.Project.Typings;
using DOM.ProjectFolders;

namespace BL;

public class ProjectManager : IProjectManager
{
    private readonly IProjectRepository _repository;
    private readonly IPbsManager _pbsManager;
    private readonly ITypeManager _typeManager;
    public string Folderpath { get; set; }
    

    public ProjectManager(IProjectRepository repository, IPbsManager pbsManager, ITypeManager typeManager)
    {
        _repository = repository;
        _pbsManager = pbsManager;
        _typeManager = typeManager;
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
        if (!_pbsManager.HasDataSaved())
        {
            _pbsManager.LoadAllPbsFiles();
        }
        
    }

    public IEnumerable<Typing> GetAllTypingsWithFullJoins()
    {
        return _typeManager.GetAllTypesWithJoin();
    }

    public void ChangeTypeEffectiveness(ICollection<TypeEffectivenessFieldChange> changes)
    {
        _pbsManager.SaveTypeEffectivenessChanges(changes);
        _pbsManager.SaveTypingsToPbsFromDatabase();
    }

    public string GetProjectFolderPath()
    {
        return Folderpath;
    }
}