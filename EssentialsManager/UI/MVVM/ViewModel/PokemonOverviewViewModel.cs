using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using BL;
using DOM.Project.Pokemons;
using DOM.Project.Typings;
using UI.Core;
using UI.MVVM.Model.Pokemon;
using UI.MVVM.Model.Type;
using UI.Services;

namespace UI.MVVM.ViewModel;

public class PokemonOverviewViewModel : Core.ViewModel
{
    private IProjectManager _projectManager;

    private INavigationService _navigation;
    private ObservableCollection<PokemonGridRow> _originalPokemonGridRows;
    private ObservableCollection<PokemonGridRow> _filteredPokemonGridRows;
    
    private string _searchTerm;
    private readonly DispatcherTimer _debounceTimer;

    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<PokemonGridRow> OriginalPokemonGridRows
    {
        get => _originalPokemonGridRows;
        set
        {
            if (Equals(value, _originalPokemonGridRows)) return;
            _originalPokemonGridRows = value;
            OnPropertyChanged();
        }
    }
    
    public ObservableCollection<PokemonGridRow> FilteredPokemonGridRows
    {
        get => _filteredPokemonGridRows;
        set
        {
            if (Equals(value, _filteredPokemonGridRows)) return;
            _filteredPokemonGridRows = value;
            OnPropertyChanged();
        }
    }
    
    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (_searchTerm != value)
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));

                // Reset and start the debounce timer
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
        }
    }

    public RelayCommand NavigateToProjectFunctionalityCommand { get; set; }
    public RelayCommand NavigateToProjectPickerCommand { get; set; }

    public PokemonOverviewViewModel(IProjectManager projectManager, INavigationService navigation)
    {
        _projectManager = projectManager;
        _navigation = navigation;

        OriginalPokemonGridRows = [];
        FilteredPokemonGridRows = [];

        NavigateToProjectFunctionalityCommand =
            new RelayCommand(t => Navigation.NavigateTo<FunctionalityOverviewViewModel>(), o => true);
        NavigateToProjectPickerCommand = new RelayCommand(param => NavigateToProjectPicker(), o => true);
        
        _debounceTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1) // Adjust the delay as needed
        };
        _debounceTimer.Tick += (s, e) => 
        {
            _debounceTimer.Stop(); // Stop the timer to prevent repeated ticks
            FilterCollection(); // Call the filtering method
        };
    }

    private void NavigateToProjectPicker()
    {
        _projectManager.ResetConnectionString();
        Navigation.NavigateTo<ProjectsPickerViewModel>();
    }

    public void ReadAllPokemon()
    {
        OriginalPokemonGridRows = [];
        if (OriginalPokemonGridRows?.Count != 0) return;
        IEnumerable<Pokemon> pokemons = _projectManager.GetAllPokemonsWithTypings();
        string projectPath = _projectManager.GetProjectFolderPath();

        string typeImagePath = projectPath + "\\Graphics\\UI\\types.png";

        if (!File.Exists(typeImagePath))
        {
            typeImagePath = projectPath + "\\Graphics\\Pictures\\Pokedex\\icon_types.png";
        }

        BitmapImage typeIconsImage = new BitmapImage(new Uri(typeImagePath));
        int amountOfTypings = _projectManager.getAmountOfTypings();
        int imageHeight = typeIconsImage.PixelHeight;
        int imageWidth = typeIconsImage.PixelWidth;
        int iconHeight = imageHeight / amountOfTypings;

        foreach (Pokemon pokemon in pokemons)
        {
            if (pokemon.FormNumber == 0)
            {
                // icon
                string pokemonName = pokemon.InternalName;
                if (pokemon.FormNumber != 0)
                {
                    pokemonName = pokemonName + "_" + pokemon.FormNumber;
                }

                string iconImageSource = projectPath + "\\Graphics\\Pokemon\\Front\\" + pokemonName + ".png";

                if (!File.Exists(iconImageSource))
                {
                    iconImageSource = projectPath + "\\Graphics\\Pokemon\\Front\\000.png";
                }

                // typing
                List<Typing> typings = pokemon.Typings.ToList();

                int yOffset = typings[0].IconPosition * iconHeight;
                TypeImage type1 = new TypeImage()
                {
                    ImagePath = typeImagePath,
                    IconSourceRect = new Int32Rect(0, yOffset, imageWidth, iconHeight),
                    TypeName = typings[0].Name
                };

                TypeImage type2 = null;

                if (typings.Count > 1)
                {
                    yOffset = typings[1].IconPosition * iconHeight;
                    type2 = new TypeImage()
                    {
                        ImagePath = typeImagePath,
                        IconSourceRect = new Int32Rect(0, yOffset, imageWidth, iconHeight),
                        TypeName = typings[1].Name
                    };
                }

                PokemonGridRow gridRow = new PokemonGridRow()
                {
                    DexNumber = pokemon.DexNumber,
                    FormNumber = pokemon.FormNumber,
                    IconImageSource = iconImageSource,
                    Name = pokemon.Name,
                    Type1 = type1,
                    Type2 = type2,
                };

                OriginalPokemonGridRows?.Add(gridRow);
            }
        }
        
        FilteredPokemonGridRows = new ObservableCollection<PokemonGridRow>(OriginalPokemonGridRows!);
        var collectionView = CollectionViewSource.GetDefaultView(FilteredPokemonGridRows!);
        if (collectionView != null)
        {
            // Clear any existing sort descriptions, in case they exist
            collectionView.SortDescriptions.Clear();

            // Add a new SortDescription for DexNumber, sorted ascending
            collectionView.SortDescriptions.Add(new SortDescription("DexNumber", ListSortDirection.Ascending));
        }
    }
    
    private void FilterCollection()
    {
        FilteredPokemonGridRows.Clear();
        var filtered = OriginalPokemonGridRows
            .Where(item => string.IsNullOrEmpty(SearchTerm) || 
                           item.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || 
                           item.Type1.TypeName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)|| 
                           item.Type2?.TypeName?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) == true
                           );

        foreach (var item in filtered)
        {
            FilteredPokemonGridRows.Add(item);
        }
    }
    
    
}