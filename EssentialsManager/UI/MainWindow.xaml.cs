using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BL;
using BL.Exceptions;
using DOM.ProjectFolders;
using Microsoft.Win32;
using UI.Templates;

namespace UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ObservableCollection<Card> Cards { get; set; }
    private IProjectFolderManager _projectFolderManager;
    private IProjectManager _projectManager;
    public MainWindow(IProjectFolderManager projectFolderManager, IProjectManager projectManager)
    {
        _projectFolderManager = projectFolderManager;
        _projectManager = projectManager;
        InitializeComponent();
        DataContext = this;

        // Retrieve data from the database
        Cards = GetDataFromDatabase();
    }
    
    private ObservableCollection<Card> GetDataFromDatabase()
    {
        // Connect to the database and retrieve data
        IEnumerable<Project> projects = _projectFolderManager.GetProjects();
        ObservableCollection<Card> cards = new ObservableCollection<Card>();
        foreach (var project in projects)
        {
            cards.Add(new Card{ Name = project.Name, Picture = project.Photo});
        }
        
        return cards;
    }

    private void btnOpen_Click(object sender, RoutedEventArgs e)
    {
        // Configure open folder dialog box
        OpenFolderDialog dialog = new();

        dialog.Multiselect = false;
        dialog.Title = "Select a folder";

        // Show open folder dialog box
        bool? result = dialog.ShowDialog();

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
                _projectManager.ChangeConnectionString(fullPathToEssentialsManager + "\\project.db", fullPathToFolder);
               
                // compile all pbs files into the database
                _projectManager.CompilePbsFiles();
                
                Cards.Add(new Card{ Name = project.Name, Picture = project.Photo });
                CardsListview.Items.Refresh();
            }
            catch (ProjectAlreadyExistException exception)
            {
                ErrorTextBlock.Text = exception.Message;
                ErrorTextBlock.Foreground = Brushes.Red;

            }
            
        }
    }
    
    
}