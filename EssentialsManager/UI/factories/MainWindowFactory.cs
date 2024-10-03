using BL;

namespace UI.factories;

public class MainWindowFactory : IMainWindowFactory
{
    private readonly IProjectFolderManager _projectFolderManager;
    private readonly IProjectManager _projectManager;

    public MainWindowFactory(IProjectFolderManager projectFolderManager, IProjectManager projectManager)
    {
        _projectFolderManager = projectFolderManager;
        _projectManager = projectManager;

    }

    public MainWindow CreateMainWindow()
    {
        return new MainWindow(_projectFolderManager, _projectManager);
    }
}