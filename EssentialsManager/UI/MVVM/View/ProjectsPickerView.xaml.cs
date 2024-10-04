using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BL;
using BL.Exceptions;
using DOM.ProjectFolders;
using Microsoft.Win32;
using UI.Templates;

namespace UI.MVVM.View;

public partial class ProjectsPickerView : UserControl
{
    public ProjectsPickerView()
    {
        InitializeComponent();
        
    }
    
}