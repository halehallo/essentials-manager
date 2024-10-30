namespace UI.Services;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

public class ImageSourceConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is string imagePath && values[1] is Int32Rect rect)
        {
            try
            {
                var bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                return new CroppedBitmap(bitmapImage, rect);
            }
            catch
            {
                // Handle errors (e.g., file not found)
                return null;
            }
        }
        return null;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
