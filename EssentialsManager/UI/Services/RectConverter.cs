using System.Windows;
using System.Windows.Data;

namespace UI.Services;

public class RectConverter : IMultiValueConverter
{
    #region IMultiValueConverter Members

    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (values == null || values.Length < 2 || 
            values[0] == DependencyProperty.UnsetValue || 
            values[1] == DependencyProperty.UnsetValue)
        {
            return new System.Windows.Rect(0, 0, 0, 0); // Return a default or fallback value
        }
        return new System.Windows.Rect(0,0, (double) values[0], (double) values[1]);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion
}