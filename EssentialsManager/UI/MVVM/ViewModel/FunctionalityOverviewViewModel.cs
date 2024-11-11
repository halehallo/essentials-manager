using BL;
using LiveCharts;
using UI.Core;
using UI.MVVM.Model.Error;
using UI.Services;

namespace UI.MVVM.ViewModel;

public class FunctionalityOverviewViewModel : Core.ViewModel
{
    private IProjectManager _projectManager;
    private INavigationService _navigation;
    private TypeEffectivenessViewModel _typeEffectivenessViewModel;
    private PokemonOverviewViewModel _pokemonOverviewViewModel;
    private DexTypeCountViewModel _dexTypeCountViewModel;

    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand NavigateToProjectPickerCommand { get; set; }
    public RelayCommand NavigateToTypeEffectivenessViewCommand { get; set; }
    public RelayCommand NavigateToPokemonOverviewCommand { get; set; }
    public RelayCommand NavigateToDexTypeCountOverviewCommand { get; set; }
    
    public SeriesCollection SeriesCollection { get; set; }
    public List<string> TypingLabels { get; set; }

    public FunctionalityOverviewViewModel(IProjectManager projectManager,
        INavigationService navigationService, TypeEffectivenessViewModel typeEffectivenessViewModel,
        PokemonOverviewViewModel pokemonOverviewViewModel, DexTypeCountViewModel dexTypeCountViewModel)
    {
        _projectManager = projectManager;
        _typeEffectivenessViewModel = typeEffectivenessViewModel;
        _pokemonOverviewViewModel = pokemonOverviewViewModel;
        _dexTypeCountViewModel = dexTypeCountViewModel;
        
        Navigation = navigationService;
        NavigateToProjectPickerCommand = new RelayCommand(param => NavigateToProjectPicker(), o => true);
        NavigateToTypeEffectivenessViewCommand = new RelayCommand(param => NavigateToTypeEffectivenessView(), o => true);
        NavigateToPokemonOverviewCommand = new RelayCommand(param => NavigateToPokemonOverview(), o => true);
        NavigateToDexTypeCountOverviewCommand = new RelayCommand(param => NavigateToDexTypeCountOverview(), o => true);
    }

    private void NavigateToProjectPicker()
    {
        _projectManager.ResetConnectionString();
        Navigation.NavigateTo<ProjectsPickerViewModel>();
    }
    
    private void NavigateToTypeEffectivenessView()
    {
        _typeEffectivenessViewModel.ReadTypeImages();
        Navigation.NavigateTo<TypeEffectivenessViewModel>();
    }
    
    private void NavigateToPokemonOverview()
    {
        _pokemonOverviewViewModel.ReadAllPokemon();
        Navigation.NavigateTo<PokemonOverviewViewModel>();
    }
    
    private void NavigateToDexTypeCountOverview()
    {
        _dexTypeCountViewModel.ReadAllDexTypeCounts();
        Navigation.NavigateTo<DexTypeCountViewModel>();
    }
}