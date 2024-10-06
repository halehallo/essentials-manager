using BL;
using UI.Core;
using UI.MVVM.Model.Error;
using UI.Services;

namespace UI.MVVM.ViewModel;

public class FunctionalityOverviewViewModel : Core.ViewModel
{
    private IProjectManager _projectManager;
    private INavigationService _navigation;
    private TypeEffectivenessViewModel _typeEffectivenessViewModel;

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

    public FunctionalityOverviewViewModel(IProjectManager projectManager,
        INavigationService navigationService, TypeEffectivenessViewModel typeEffectivenessViewModel)
    {
        _projectManager = projectManager;
        _typeEffectivenessViewModel = typeEffectivenessViewModel;
        
        Navigation = navigationService;
        NavigateToProjectPickerCommand = new RelayCommand(param => NavigateToProjectFunctionality(), o => true);
        NavigateToTypeEffectivenessViewCommand = new RelayCommand(param => NavigateToTypeEffectivenessView(), o => true);
    }

    private void NavigateToProjectFunctionality()
    {
        _projectManager.ResetConnectionString();
        Navigation.NavigateTo<ProjectsPickerViewModel>();
    }
    
    private void NavigateToTypeEffectivenessView()
    {
        _typeEffectivenessViewModel.ReadTypeImages();
        Navigation.NavigateTo<TypeEffectivenessViewModel>();
    }
}