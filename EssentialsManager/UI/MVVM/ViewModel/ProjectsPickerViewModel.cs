using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using BL;
using BL.Exceptions;
using DOM.ProjectFolders;
using Microsoft.Win32;
using UI.Core;
using UI.MVVM.Model;
using UI.MVVM.Model.Error;
using UI.Services;

namespace UI.MVVM.ViewModel;

public class ProjectsPickerViewModel : Core.ViewModel
{
    private IProjectFolderManager _projectFolderManager;
    private IProjectManager _projectManager;
    
    private INavigationService _navigation;

    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }
    
    public RelayCommand NavigateToProjectFunctionalityCommand { get; set; }
    
    public ObservableCollection<ProjectCard> Cards { get; set; }
    private ErrorTextBlock _errorTextBlock;
    public ICommand OpenFolderCommand { get; }

    public ErrorTextBlock ErrorTextBlock
    {
        get => _errorTextBlock;
        set
        {
            _errorTextBlock = value;
            OnPropertyChanged();
        }
    }

    public ProjectsPickerViewModel(IProjectFolderManager projectFolderManager, IProjectManager projectManager, INavigationService navigationService)
    {
        _projectFolderManager = projectFolderManager;
        _projectManager = projectManager;
        
        Navigation = navigationService;
        NavigateToProjectFunctionalityCommand = new RelayCommand(NavigateToProjectFunctionality, o => true);

        ErrorTextBlock = new ErrorTextBlock()
        {
            Text = "no errors",
            Foreground = Brushes.Green
        };

        
        
        // Retrieve data from the database
        Cards = GetDataFromDatabase();
        
        OpenFolderCommand = new RelayCommand(param => OpenFolder(), o => true);
        
        // SetTestData();
        
    }

    private void SetTestData()
    {
        for (int i = 1; i < 5; i++)
        {
            Cards.Add(new ProjectCard()
            {
                Name = $"Test Card {i}",
                Picture = "C:\\Users\\hanne\\private\\pokemon\\manager_app\\testing\\Pokemon Essentials v21.1 2023-07-30\\Graphics\\Titles\\title.png"
            });
        }
        
    }

    private void NavigateToProjectFunctionality(object projectCardObj)
    {
        if (projectCardObj is ProjectCard projectCard)
        {
            _projectManager.ChangeConnectionString(projectCard.FolderPath);
            Navigation.NavigateTo<FunctionalityOverviewViewModel>();
        }
        
    }
    
    private ObservableCollection<ProjectCard> GetDataFromDatabase()
    {
        // Connect to the database and retrieve data
        IEnumerable<Project> projects = _projectFolderManager.GetProjects();
        ObservableCollection<ProjectCard> cards = new ObservableCollection<ProjectCard>();
        foreach (var project in projects)
        {
            cards.Add(new ProjectCard{ Name = project.Name, Picture = project.Photo, FolderPath = project.FolderPath + "\\EssentialsManager\\project.db"});
        }
        
        return cards;
    }

    private void OpenFolder()
    {
        // Configure open folder dialog box
        OpenFolderDialog dialog = new()
        {
            Multiselect = false,
            Title = "Select a folder"
        };

        // Show open folder dialog box
        bool? result = dialog.ShowDialog();

        if (result == false || result == null)
        {
            ErrorTextBlock.Text = "no folder selected";
            ErrorTextBlock.Foreground = Brushes.Red;
        }
        // Process open folder dialog box results
        if (result == true)
        {
            // Get the selected folder
            string fullPathToFolder = dialog.FolderName;
            string folderNameOnly = dialog.SafeFolderName;
            string fullPathToEssentialsManager = fullPathToFolder + "\\EssentialsManager";
            
            // create essentialsmanager folder if it doesnt exist
            if (!Directory.Exists(fullPathToEssentialsManager))
            {
                // If it doesn't exist, create it
                Directory.CreateDirectory(fullPathToEssentialsManager);
                Console.WriteLine("Folder created successfully.");
            }
            
            // add the project to the database of projects
            try
            {
                ErrorTextBlock.Text = "No Errors";
                ErrorTextBlock.Foreground = Brushes.Green;
                
                Project project = _projectFolderManager.AddProject(folderNameOnly, fullPathToFolder);
                
                // change the connection of the projects dbcontext to the newly selected project
                _projectManager.ChangeConnectionString(fullPathToFolder);
                
                // compile all pbs files into the database
                //TODO: uncomment but fix behavior
                _projectManager.CompilePbsFiles();
                
                Cards.Add(new ProjectCard{ Name = project.Name, Picture = project.Photo, FolderPath = fullPathToFolder });
                // CardsListview.Items.Refresh();
                _projectManager.ResetConnectionString();
            }
            catch (ProjectAlreadyExistException exception)
            {
                ErrorTextBlock.Text = exception.Message;
                ErrorTextBlock.Foreground = Brushes.Red;

            }
            
        }
    }
}