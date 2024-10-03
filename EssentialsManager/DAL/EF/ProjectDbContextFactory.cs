using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class ProjectDbContextFactory
{
    public static ProjectDbContext CreateDbContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
        optionsBuilder.UseSqlite(connectionString);
        
        var options = optionsBuilder.Options;
        return new ProjectDbContext(options);
    }
}