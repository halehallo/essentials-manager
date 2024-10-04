using UI.Core;
using UI.Services;

namespace UI.MVVM.ViewModel;

public class MainViewModel : Core.ViewModel
{
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
    
    public RelayCommand NavigateToHomeCommand { get; set; }

    public MainViewModel(INavigationService navigationService)
    {
        Navigation = navigationService;
        NavigateToHomeCommand = new RelayCommand(o => Navigation.NavigateTo<ProjectsPickerViewModel>(), o => true);
    }
}