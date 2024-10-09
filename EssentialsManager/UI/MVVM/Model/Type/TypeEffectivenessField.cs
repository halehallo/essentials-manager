using System.Windows.Media;
using UI.Core;

namespace UI.MVVM.Model.Type;

public class TypeEffectivenessField : ObservableObject
{
    private string _text = "1";
    private int _state = 0;
    private string _stateText = "Weakness";
    private Brush _backgroundColor = Brushes.LightGray;
    private Brush _foregroundColor = Brushes.Black;
    private int _initialState = 0;

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

    public int State
    {
        get => _state;
        set
        {
            if (value == _state) return;
            _state = value;
            OnPropertyChanged();
        }
    }
    
    public string StateText
    {
        get => _stateText;
        set
        {
            if (value == _stateText) return;
            _stateText = value;
            OnPropertyChanged();
        }
    }

    public int InitialState
    {
        get => _initialState;
        set
        {
            if (value == _initialState) return;
            _initialState = value;
            OnPropertyChanged();
        }
    }

    public Brush BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            if (Equals(value, _backgroundColor)) return;
            _backgroundColor = value;
            OnPropertyChanged();
        }
    }

    public Brush ForegroundColor
    {
        get => _foregroundColor;
        set
        {
            if (Equals(value, _foregroundColor)) return;
            _foregroundColor = value;
            OnPropertyChanged();
        }
    }
}