using BL.Exceptions;
using DAL;
using DOM.ProjectFolders;
using Microsoft.Extensions.Logging;

namespace BL;

public class ProjectFolderManager : IProjectFolderManager
{
    private IProjectFoldersRepository _repository;
    private IFileManager _fileManager;
    private readonly ILogger<ProjectFolderManager> _logger;

    public ProjectFolderManager(IProjectFoldersRepository repository, IFileManager fileManager,
        ILogger<ProjectFolderManager> logger)
    {
        _repository = repository;
        _fileManager = fileManager;
        _logger = logger;
    }

    public IEnumerable<Project> GetProjects()
    {
        return _repository.ReadAllProjects();
    }

    public Project AddProject(string name, string folderpath)
    {
        // check if project already in database
        if (_repository.ReadProjectByNameAndFolderPath(name, folderpath) != null)
        {
            throw new ProjectAlreadyExistException("The specified projectfolder is already in the list of saved project" +
                                                   " by the essentials manager");
        }
        // add project to database if not in there
        string uriTitleImage = folderpath + "\\Graphics\\Titles\\title.png";
        try
        {
            _fileManager.CheckIfIsEssentialsProjectFolder(folderpath);
            _fileManager.CheckIfImageExists(uriTitleImage);
        }
        catch (WrongImageException e)
        {
            _logger.LogInformation("Something went wrong with the image. Using default image");
            _logger.LogInformation(e.Message);
            uriTitleImage = "../../../../Images/title.png";
        }
        Project project = new Project(name, folderpath, uriTitleImage);
        return _repository.CreateProject(project);
    }
}