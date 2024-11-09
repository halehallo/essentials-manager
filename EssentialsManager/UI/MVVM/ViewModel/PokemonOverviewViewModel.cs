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
    private ICollectionView _pokemonView;
    
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
    
    public ICollectionView PokemonView
    {
        get => _pokemonView;
        private set
        {
            if (Equals(value, _pokemonView)) return;
            _pokemonView = value;
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
        PokemonView = CollectionViewSource.GetDefaultView(OriginalPokemonGridRows);
        PokemonView.Filter = FilterPredicate;

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
            PokemonView.Refresh(); // Call the filtering method
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

        int pokemonId = 0;
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
                    Id = pokemonId++,
                    DexNumber = pokemon.DexNumber,
                    FormNumber = pokemon.FormNumber,
                    IconImageSource = iconImageSource,
                    Name = pokemon.Name,
                    Type1 = type1,
                    Type2 = type2,
                    IsCatchable = pokemon.IsCatchable,
                    IsGift = pokemon.IsGift,
                    IsChanged = false,
                };

                OriginalPokemonGridRows?.Add(gridRow);
            }
        }
        PokemonView = CollectionViewSource.GetDefaultView(OriginalPokemonGridRows!);
        PokemonView.Filter = FilterPredicate;
        
        var collectionView = PokemonView;
        if (collectionView != null)
        {
            // Clear any existing sort descriptions, in case they exist
            collectionView.SortDescriptions.Clear();

            // Add a new SortDescription for DexNumber, sorted ascending
            collectionView.SortDescriptions.Add(new SortDescription("DexNumber", ListSortDirection.Ascending));
        }
    }
    private bool FilterPredicate(object item)
    {
        if (item is not PokemonGridRow pokemon) return false;

        // Apply filtering logic based on SearchTerm
        return string.IsNullOrEmpty(SearchTerm) ||
               pokemon.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
               pokemon.Type1.TypeName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
               (pokemon.Type2?.TypeName?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false);
    }

    
    
}