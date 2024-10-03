using DOM.ProjectFolders;

namespace DAL.EF;

public static class ProjectFoldersInitializer
{
    public static bool IsInitialized { get; set; }
    
    public static void Initialize(ProjectFoldersDbContext context, bool dropCreateDatabase = false)
    {
        if (dropCreateDatabase)
        {
            context.Database.EnsureDeleted();
            IsInitialized = false;
        }

        if (context.Database.EnsureCreated())
        {
            Seed(context);
            IsInitialized = true;
        }
    }

    private static void Seed(ProjectFoldersDbContext context)
    {
        //initializing elements
        //
        // Project testProject = new Project("test", "C:\\Users\\hanne\\Pictures", "C:\\Users\\hanne\\Pictures\\image1.png");
        
        //linking elements
        
        //tractor-farmer
        // alfred.Tractors.Add(t5Gv);
        // t5Gv.Farmer = alfred;
        // alfred.Tractors.Add(t7R350);
        // t7R350.Farmer = alfred;
        
        //adding elements to dbContext
        //
        // context.Tractors.Add(t9470Rx);
        // context.Tractors.Add(t5Gv);
        // context.Tractors.Add(t7R270);
        // context.Tractors.Add(t7R350);
        // context.Tractors.Add(t7R420);
        // context.Projects.Add(testProject);
        
        //saving changes
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
}