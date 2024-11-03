using System.Windows;
using UI.Core;

namespace UI.MVVM.Model.Type;

public class TypeImage : ObservableObject
{
    private string _imagePath;
    private Int32Rect _iconSourceRect;
    private string _typeName;

    public string ImagePath
    {
        get => _imagePath;
        set
        {
            if (value == _imagePath) return;
            _imagePath = value;
            OnPropertyChanged();
        }
    }

    public Int32Rect IconSourceRect
    {
        get => _iconSourceRect;
        set
        {
            if (value.Equals(_iconSourceRect)) return;
            _iconSourceRect = value;
            OnPropertyChanged();
        }
    }
    
    public string TypeName
    {
        get => _typeName;
        set
        {
            if (value == _typeName) return;
            _typeName = value;
            OnPropertyChanged();
        }
    }
}