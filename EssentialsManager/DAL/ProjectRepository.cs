using DAL.EF;
using DOM.Project;
using DOM.ProjectFolders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectDbContext _context;
    //TODO: fix the logger method needing serviceprovider.
    private readonly ILogger _logger;

    public ProjectRepository(ProjectDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        _logger = factory.CreateLogger("ProjectRepository");
    }
    
    public bool UpdateConnectionString(string connectionString)
    {
        try
        {
            //new context so the database initializes in case of no file
            _context.Database.CloseConnection();
            ProjectDbContext disposableDbContext = ProjectDbContextFactory.CreateDbContext(connectionString);
            disposableDbContext.Dispose();
            
            _context.Database.GetDbConnection().ConnectionString = connectionString;
            _context.Database.OpenConnection();
            Console.WriteLine(_context.Database.GetDbConnection().ConnectionString);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Something went wrong while changing the database connection");
            return false;
        }
        
    }
}