using UI.Core;

namespace UI.MVVM.Model.Dex;

public class DexTypeCountObservableObject : ObservableObject
{
    private string _type;
    private int _count;

    public string Type
    {
        get => _type;
        set
        {
            if (value == _type) return;
            _type = value;
            OnPropertyChanged();
        }
    }

    public int Count
    {
        get => _count;
        set
        {
            if (value == _count) return;
            _count = value;
            OnPropertyChanged();
        }
    }
}