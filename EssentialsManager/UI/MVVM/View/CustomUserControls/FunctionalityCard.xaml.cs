using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI.MVVM.View.CustomUserControls;

public partial class FunctionalityCard : UserControl
{
    public FunctionalityCard()
    {
        InitializeComponent();
    }
    
    // Dependency Property for BackgroundColor
    public static readonly DependencyProperty BackgroundColorProperty = 
        DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(FunctionalityCard), new PropertyMetadata(Brushes.Transparent));

    public Brush BackgroundColor
    {
        get { return (Brush)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }

    // Dependency Property for Text
    public static readonly DependencyProperty TextProperty = 
        DependencyProperty.Register("Text", typeof(string), typeof(FunctionalityCard), new PropertyMetadata(string.Empty));

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }
}