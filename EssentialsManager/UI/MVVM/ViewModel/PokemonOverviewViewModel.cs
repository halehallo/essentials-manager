using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
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
    private ObservableCollection<PokemonGridRow> _pokemonGridRows;


    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<PokemonGridRow> PokemonGridRows
    {
        get => _pokemonGridRows;
        set
        {
            if (Equals(value, _pokemonGridRows)) return;
            _pokemonGridRows = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand NavigateToProjectFunctionalityCommand { get; set; }
    public RelayCommand NavigateToProjectPickerCommand { get; set; }

    public PokemonOverviewViewModel(IProjectManager projectManager, INavigationService navigation)
    {
        _projectManager = projectManager;
        _navigation = navigation;

        PokemonGridRows = [];

        NavigateToProjectFunctionalityCommand =
            new RelayCommand(t => Navigation.NavigateTo<FunctionalityOverviewViewModel>(), o => true);
        NavigateToProjectPickerCommand = new RelayCommand(param => NavigateToProjectPicker(), o => true);
    }

    private void NavigateToProjectPicker()
    {
        _projectManager.ResetConnectionString();
        Navigation.NavigateTo<ProjectsPickerViewModel>();
    }

    public void ReadAllPokemon()
    {
        PokemonGridRows = [];
        if (PokemonGridRows?.Count != 0) return;
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
                    IconSourceRect = new Int32Rect(0, yOffset, imageWidth, iconHeight)
                };

                TypeImage type2 = null;

                if (typings.Count > 1)
                {
                    yOffset = typings[1].IconPosition * iconHeight;
                    type2 = new TypeImage()
                    {
                        ImagePath = typeImagePath,
                        IconSourceRect = new Int32Rect(0, yOffset, imageWidth, iconHeight)
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

                PokemonGridRows?.Add(gridRow);
            }
        }
        var collectionView = CollectionViewSource.GetDefaultView(PokemonGridRows!);
        if (collectionView != null)
        {
            // Clear any existing sort descriptions, in case they exist
            collectionView.SortDescriptions.Clear();

            // Add a new SortDescription for DexNumber, sorted ascending
            collectionView.SortDescriptions.Add(new SortDescription("DexNumber", ListSortDirection.Ascending));
        }
    }
}