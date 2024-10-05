using System.Windows.Media;
using UI.Core;

namespace UI.MVVM.Model.Error;

public class ErrorTextBlock : ObservableObject
{
    private string _text;
    private Brush _foreground;

    public string Text
    {
        get => _text;
        set
        {
            if (value == _text) return;
            _text = value;
            OnPropertyChanged();
        }
    }

    public Brush Foreground
    {
        get => _foreground;
        set
        {
            if (Equals(value, _foreground)) return;
            _foreground = value;
            OnPropertyChanged();
        }
    }
}