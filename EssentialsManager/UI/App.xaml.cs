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
using UI.factories;

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
        // Add scoped services
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
        services.AddScoped<IMainWindowFactory, MainWindowFactory>();
        
        
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
        var mainWindowFactory = _serviceProvider.GetRequiredService<IMainWindowFactory>();
        var mainWindow = mainWindowFactory.CreateMainWindow();
        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // Dispose the service provider when the application exits
        _serviceProvider.Dispose();
        base.OnExit(e);
    }
}

