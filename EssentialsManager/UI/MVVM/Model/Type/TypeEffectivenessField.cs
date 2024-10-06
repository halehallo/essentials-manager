using UI.Core;

namespace UI.MVVM.Model.Type;

public class TypeEffectivenessField : ObservableObject
{
    private string _text = "1";

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
}