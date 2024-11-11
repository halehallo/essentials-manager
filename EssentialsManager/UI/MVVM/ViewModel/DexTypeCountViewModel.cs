using System.Collections.ObjectModel;
using BL;
using BL.DataTransferObjects;
using BL.PbsManagers.Dex;
using LiveCharts;
using LiveCharts.Wpf;
using UI.Core;
using UI.Services;

namespace UI.MVVM.ViewModel;

public class DexTypeCountViewModel : Core.ViewModel
{
    private IProjectManager _projectManager;
    
    private INavigationService _navigation;
    
    private readonly IDexManager _dexManager;
    
    public SeriesCollection SeriesCollection { get; set; }
    public ObservableCollection<string> TypingObjectLabels { get; set; }

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
    public RelayCommand NavigateToProjectPickerCommand { get; set; }
    
    public DexTypeCountViewModel(IProjectManager projectManager, INavigationService navigationService, IDexManager dexManager)
    {
        _projectManager = projectManager;
        Navigation = navigationService;
        _dexManager = dexManager;
        
        NavigateToProjectFunctionalityCommand = new RelayCommand(t => Navigation.NavigateTo<FunctionalityOverviewViewModel>(), o => true);
        NavigateToProjectPickerCommand = new RelayCommand(param => NavigateToProjectPicker(), o => true);
    }
    
    private void NavigateToProjectPicker()
    {
        _projectManager.ResetConnectionString();
        Navigation.NavigateTo<ProjectsPickerViewModel>();
    }

    public void ReadAllDexTypeCounts()
    {
        var typingObjectData = _dexManager.GetAllTypeCounts();

        // Set up labels and series data for the chart
        var dexTypeCountObjects = typingObjectData as DexTypeCountObject[] ?? typingObjectData.ToArray();
        TypingObjectLabels = new ObservableCollection<string>(dexTypeCountObjects.Select(x => x.Type));
        SeriesCollection = new SeriesCollection();

        // Populate the SeriesCollection with a PieSeries for each type
        foreach (var typeCount in dexTypeCountObjects)
        {
            SeriesCollection.Add(new PieSeries
            {
                Title = typeCount.Type, // Set the title to the Pokémon type
                Values = new ChartValues<int> { typeCount.Count }, // Set the count as the value
                DataLabels = true, // Enable labels
                LabelPoint = chartPoint => $"{chartPoint.SeriesView.Title}: {chartPoint.Y}" // Format label
            });
        }
    }
}