using UI.Core;
using UI.MVVM.ViewModel;

namespace UI.Services;

public class NavigationService : ObservableObject, INavigationService
{
    private readonly Func<Type, ViewModel> _factory;
    private ViewModel _currentView;

    public ViewModel CurrentView
    {
        get => _currentView;
        private set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public NavigationService(Func<Type, ViewModel> factory)
    {
        _factory = factory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModel
    {
        ViewModel viewModel = _factory.Invoke(typeof(TViewModel));
        CurrentView = viewModel;
    }
}