using DOM.Project;
using DOM.ProjectFolders;
using System.Collections.Generic;

namespace DAL.EF;

public static class ProjectInitializer
{
    public static bool IsInitialized { get; set; }
    
    public static void Initialize(ProjectDbContext context, bool dropCreateDatabase = false)
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

    private static void Seed(ProjectDbContext context)
    {
        //initializing elements
        // Typing typing1 = new Typing("test1", "test1", 1, false, false,
        //     new List<string> { "test1", "test2" }, new List<string> { "test1", "test2" },
        //     new List<string>());
        // Typing typing3 = new Typing("test3", "test2", 1, false, false,
        //     new List<string> { "test1", "test2" }, new List<string> { "test1", "test2" },
        //     new List<string>());
        // Typing typing2 = new Typing("test2", "test3", 1, false, false,
        //     new List<string>(), new List<string>(), new List<string>());
        //
        // TypingResistance res11 = new TypingResistance() { Typing = typing1, Resistance = typing1 };
        // TypingResistance res12 = new TypingResistance() { Typing = typing1, Resistance = typing2 };
        // TypingWeakness weak11 = new TypingWeakness() { Typing = typing1, Weakness  = typing1 };
        // TypingWeakness weak12 = new TypingWeakness() { Typing = typing1, Weakness  = typing2 };
        //
        // TypingResistance res31 = new TypingResistance() { Typing = typing3, Resistance = typing1 };
        // TypingResistance res32 = new TypingResistance() { Typing = typing3, Resistance = typing2 };
        // TypingWeakness weak31 = new TypingWeakness() { Typing = typing3, Weakness  = typing1 };
        // TypingWeakness weak32 = new TypingWeakness() { Typing = typing3, Weakness  = typing2 };
        //
        // //linking elements
        // typing1.Resistances.Add(res11);
        // typing1.Resistances.Add(res12);
        // typing1.Weaknesses.Add(weak11);
        // typing1.Weaknesses.Add(weak12);
        //
        // typing3.Resistances.Add(res31);
        // typing3.Resistances.Add(res32);
        // typing3.Weaknesses.Add(weak31);
        // typing3.Weaknesses.Add(weak32);
        //
        // //adding elements to dbContext
        // context.Types.Add(typing1);
        // context.Types.Add(typing2);
        // context.Types.Add(typing3);
        //
        // context.TypeWeaknesses.Add(weak11);
        // context.TypeWeaknesses.Add(weak12);
        // context.TypeWeaknesses.Add(weak31);
        // context.TypeWeaknesses.Add(weak32);
        //
        // context.TypeResistances.Add(res11);
        // context.TypeResistances.Add(res12);
        // context.TypeResistances.Add(res31);
        // context.TypeResistances.Add(res32);
        
        //saving changes
        context.SaveChanges();
        context.ChangeTracker.Clear();
    }
}