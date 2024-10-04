using UI.Core;

namespace UI.Services;

public interface INavigationService
{
    ViewModel CurrentView{get;}
    void NavigateTo<T>() where T : ViewModel;
}