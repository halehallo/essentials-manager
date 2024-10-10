using System.Diagnostics;
using DOM.ProjectFolders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.EF;

public class ProjectFoldersDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    
    public ProjectFoldersDbContext(DbContextOptions<ProjectFoldersDbContext> options) : base(options)
    {
        ProjectFoldersInitializer.Initialize(this, true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) {
            // configurations if no options are provided via the constructor
            // eg. Data provider and data source
            optionsBuilder.UseSqlite("Data Source=../../../../Projects.db");
        }
        // Configurations that should always be applied
        // ...
        optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}