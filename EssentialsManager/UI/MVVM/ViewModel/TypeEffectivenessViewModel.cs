
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using BL;
using DOM.Project.Typings;
using UI.MVVM.Model;
using UI.MVVM.Model.Type;

namespace UI.MVVM.ViewModel;

public class TypeEffectivenessViewModel : Core.ViewModel
{
    private IProjectManager _projectManager;
    
    public BitmapImage TypeIconsImage;
    private ObservableCollection<ObservableCollection<TypeEffectivenessField>> _typeEffectivenessGrid;
    public ObservableCollection<TypeImage> TypeImages {get; set;}
    public int AmountOfTypings {get; set;}

    public ObservableCollection<ObservableCollection<TypeEffectivenessField>> TypeEffectivenessGrid
    {
        get => _typeEffectivenessGrid;
        set
        {
            if (Equals(value, _typeEffectivenessGrid)) return;
            _typeEffectivenessGrid = value;
            OnPropertyChanged();
        }
    }

    public TypeEffectivenessViewModel(IProjectManager projectManager)
    {
        _projectManager = projectManager;
    }

    public void ReadTypeImages()
    {
        TypeImages = new ObservableCollection<TypeImage>();
        TypeEffectivenessGrid = new ObservableCollection<ObservableCollection<TypeEffectivenessField>>();
        IEnumerable<Typing> typings = _projectManager.GetAllTypingsWithFullJoins();
        typings = typings.OrderBy(t => t.IconPosition);
        var typingList = typings.ToList();
        AmountOfTypings = typingList.Count();
        string imagePath = _projectManager.GetProjectFolderPath() + "\\Graphics\\UI\\types.png" ;
        TypeIconsImage = new BitmapImage(new Uri(imagePath));
        int imageHeight = TypeIconsImage.PixelHeight;
        int imageWidth = TypeIconsImage.PixelWidth;
        int iconHeight = imageHeight / AmountOfTypings;

        for (int i = 0; i < AmountOfTypings; i++)
        {
            TypeEffectivenessGrid.Add(new ObservableCollection<TypeEffectivenessField>());
            for (int j = 0; j < AmountOfTypings; j++)
            {
                TypeEffectivenessGrid[i].Add(new TypeEffectivenessField()); 
            }
            Typing typing = typingList[i];
            int yOffset = typing.IconPosition * iconHeight;
            
            TypeImages.Add(new TypeImage()
            {
                ImagePath = imagePath,
                IconSourceRect = new Int32Rect (0, yOffset, imageWidth, iconHeight)
            });

            foreach (var weakness in typing.Weaknesses)
            {
                TypeEffectivenessGrid[i][weakness.Weakness.IconPosition].Text = "2";
            }
            
            foreach (var resistance in typing.Resistances)
            {
                TypeEffectivenessGrid[i][resistance.Resistance.IconPosition].Text = "1/2";
            }
            
            foreach (var immunity in typing.Immunities)
            {
                TypeEffectivenessGrid[i][immunity.Immunity.IconPosition].Text = "0";
            }
        }
    }
}