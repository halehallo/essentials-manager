using System.Windows;
using BL;
using BL.PbsManagers;
using BL.PbsManagers.Abilities;
using BL.PbsManagers.Items;
using BL.PbsManagers.Moves;
using BL.PbsManagers.Pokemons;
using BL.PbsManagers.Types;
using DAL;
using DAL.EF;
using DAL.PbsRepositories.Abilities;
using DAL.PbsRepositories.Items;
using DAL.PbsRepositories.Moves;
using DAL.PbsRepositories.Pokemons;
using DAL.PbsRepositories.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UI.Core;
using UI.MVVM.View;
using UI.MVVM.ViewModel;
using UI.Services;
using RectConverter = UI.Services.RectConverter;

namespace UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;
    
    public App()
    {
        // Configure services
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        // Build service provider
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
    
    private void ConfigureServices(IServiceCollection services)
    {
        // Add scoped services backend
        services.AddDbContext<ProjectFoldersDbContext>((optionsBuilder) =>
            optionsBuilder.UseSqlite("Data Source=../../../../Projects.db"));
        services.AddScoped<IProjectFoldersRepository, ProjectFoldersRepository>();
        services.AddScoped<IProjectFolderManager, ProjectFolderManager>();
        services.AddDbContext<ProjectDbContext>((optionsBuilder) =>
            optionsBuilder.UseSqlite("Data Source=defaultProjectDatabase.db"));
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITypingRepository, TypingRepository>();
        services.AddScoped<IAbilityRepository, AbilityRepository>();
        services.AddScoped<IMoveRepository, MoveRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IPokemonRepository, PokemonRepository>();
        services.AddScoped<IProjectManager, ProjectManager>();
        services.AddScoped<IPbsManager, PbsManager>();
        services.AddScoped<ITypeManager, TypeManager>();
        services.AddScoped<IAbilityManager, AbilityManager>();
        services.AddScoped<IMoveManager, MoveManager>();
        services.AddScoped<IItemManager, ItemManager>();
        services.AddScoped<IPokemonManager, PokemonManager>();
        services.AddScoped<IFileManager, FileManager>();
        //
        // Add scoped services frontend
        services.AddSingleton<MainWindow>(serviceProvider => new MainWindow()
        {
            DataContext = serviceProvider.GetService<MainViewModel>()
        });
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ProjectsPickerViewModel>();
        services.AddSingleton<FunctionalityOverviewViewModel>();
        services.AddSingleton<TypeEffectivenessViewModel>();
        services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetService(viewModelType));
        
        services.AddSingleton<RectConverter>();
        
        services.AddLogging(configure =>
        {
            configure.AddConsole();
            configure.SetMinimumLevel(LogLevel.Warning);
        });
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // Resolve main window and show it
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        
        // Set initial view to the project picker
        var navigation = _serviceProvider.GetRequiredService<INavigationService>();
        //TODO: uncomment for right start point
        navigation.NavigateTo<ProjectsPickerViewModel>();
        // navigation.NavigateTo<FunctionalityOverviewViewModel>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // Dispose the service provider when the application exits
        _serviceProvider.Dispose();
        base.OnExit(e);
    }
}

