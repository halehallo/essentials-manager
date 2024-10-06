using BL;
using UI.Core;
using UI.MVVM.Model.Error;
using UI.Services;

namespace UI.MVVM.ViewModel;

public class FunctionalityOverviewViewModel : Core.ViewModel
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

    public RelayCommand NavigateToProjectPickerCommand { get; set; }
    public RelayCommand NavigateToTypeEffectivenessViewCommand { get; set; }

    public FunctionalityOverviewViewModel(IProjectFolderManager projectFolderManager, IProjectManager projectManager,
        INavigationService navigationService)
    {
        _projectFolderManager = projectFolderManager;
        _projectManager = projectManager;

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
        //TODO: maybe need to read data from database here, i dont know yet
        Navigation.NavigateTo<TypeEffectivenessViewModel>();
    }
}