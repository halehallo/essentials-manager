
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BL;
using BL.DataTransferObjects;
using DOM.Project.Typings;
using UI.Core;
using UI.MVVM.Model;
using UI.MVVM.Model.Type;

namespace UI.MVVM.ViewModel;

public class TypeEffectivenessViewModel : Core.ViewModel
{
    private IProjectManager _projectManager;
    
    public BitmapImage TypeIconsImage;
    private ObservableCollection<ObservableCollection<TypeEffectivenessField>> _typeEffectivenessGrid;
    public ObservableCollection<TypeImage> TypeImages {get; set;}
    
    private TypeEffectivenessField[] fieldStates;
    public int AmountOfTypings {get; set;}
    public RelayCommand TypeEffectivenessFieldCommand { get; set; }
    public RelayCommand SaveTypeEffectivenessesCommand { get; set; }
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
        fieldStates = new TypeEffectivenessField[4];
        fieldStates[0] = new TypeEffectivenessField()
        {
            Text = "1",
            StateText = "None",
            BackgroundColor  = Brushes.LightGray,
            ForegroundColor  = Brushes.Black
        };
        fieldStates[1] = new TypeEffectivenessField()
        {
            Text = "2",
            StateText = "Weakness",
            BackgroundColor  = Brushes.LightGreen,
            ForegroundColor  = Brushes.Black
        };
        fieldStates[2] = new TypeEffectivenessField()
        {
            Text = "0",
            StateText = "Immunity",
            BackgroundColor  = Brushes.LightCoral,
            ForegroundColor  = Brushes.Black
        };
        fieldStates[3] = new TypeEffectivenessField()
        {
            Text = "1/2",
            StateText = "Resistance",
            BackgroundColor  = Brushes.LightSalmon,
            ForegroundColor  = Brushes.Black
        };
        TypeEffectivenessFieldCommand = new RelayCommand(ChangeTypeEffectivenessFieldState, o => true);
        SaveTypeEffectivenessesCommand = new RelayCommand(o => SaveTypeEffectivenessGrid(), o => true);
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
                TypeEffectivenessField field = TypeEffectivenessGrid[i][weakness.Weakness.IconPosition];
                field.State = 1;
                field.InitialState = 1;
                field.Text = "2";
                field.BackgroundColor = Brushes.LightGreen;
            }
            
            foreach (var immunity in typing.Immunities)
            {
                TypeEffectivenessField field = TypeEffectivenessGrid[i][immunity.Immunity.IconPosition];
                field.State = 2;
                field.InitialState = 2;
                field.Text = "0";
                field.BackgroundColor = Brushes.LightCoral;
                
            }
            
            foreach (var resistance in typing.Resistances)
            {
                TypeEffectivenessField field = TypeEffectivenessGrid[i][resistance.Resistance.IconPosition];
                field.State = 3;
                field.InitialState = 3;
                field.Text = "1/2";
                field.BackgroundColor = Brushes.LightSalmon;
                
            }
        }
    }

    public void ChangeTypeEffectivenessFieldState(object fieldObj)
    {
        if (fieldObj is TypeEffectivenessField field)
        {
            int fieldState = (field.State + 1) % 4;
            field.State = fieldState;
            field.Text = fieldStates[fieldState].Text;
            field.BackgroundColor = fieldStates[fieldState].BackgroundColor;
            field.ForegroundColor = fieldStates[fieldState].ForegroundColor;
            
        }
    }

    public void SaveTypeEffectivenessGrid()
    {
        List<TypeEffectivenessFieldChange> changedFields = new List<TypeEffectivenessFieldChange>();
        for (int i = 0; i < AmountOfTypings; i++)
        {
            for (int j = 0; j < AmountOfTypings; j++)
            {
                var field = TypeEffectivenessGrid[i][j];
                if (field.State != field.InitialState)
                {
                    changedFields.Add(new TypeEffectivenessFieldChange()
                    {
                        InitialState = fieldStates[field.InitialState].StateText,
                        State = fieldStates[field.State].StateText,
                        AttackingType = j,
                        DefendingType = i
                    });
                }
            }
        }
        _projectManager.ChangeTypeEffectiveness(changedFields);
    }
}