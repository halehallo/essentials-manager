using UI.Core;
using UI.MVVM.Model.Type;

namespace UI.MVVM.Model.Pokemon;

public class PokemonGridRow : ObservableObject
{
    private string _iconImageSource;
    private string _name;
    private TypeImage _type1;
    private TypeImage _type2;
    private int _dexNumber;
    private int _formNumber;
    private bool _isCatchable;
    private bool _isGift;
    private bool _isChanged;

    public string IconImageSource
    {
        get => _iconImageSource;
        set
        {
            if (value == _iconImageSource) return;
            _iconImageSource = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }

    public TypeImage Type1
    {
        get => _type1;
        set
        {
            if (Equals(value, _type1)) return;
            _type1 = value;
            OnPropertyChanged();
        }
    }

    public TypeImage Type2
    {
        get => _type2;
        set
        {
            if (Equals(value, _type2)) return;
            _type2 = value;
            OnPropertyChanged();
        }
    }

    public int DexNumber
    {
        get => _dexNumber;
        set
        {
            if (value == _dexNumber) return;
            _dexNumber = value;
            OnPropertyChanged();
        }
    }

    public int FormNumber
    {
        get => _formNumber;
        set
        {
            if (value == _formNumber) return;
            _formNumber = value;
            OnPropertyChanged();
        }
    }

    public bool IsCatchable
    {
        get => _isCatchable;
        set
        {
            if (value == _isCatchable) return;
            _isCatchable = value;
            OnPropertyChanged();
        }
    }

    public bool IsGift
    {
        get => _isGift;
        set
        {
            if (value == _isGift) return;
            _isGift = value;
            _isChanged = true;
            OnPropertyChanged();
        }
    }

    public bool IsChanged
    {
        get => _isChanged;
        set
        {
            if (value == _isChanged) return;
            _isChanged = value;
            OnPropertyChanged();
        }
    }
}