using System.Diagnostics;
using DOM.Project.Abilities;
using DOM.Project.Items;
using DOM.Project.Moves;
using DOM.Project.Pokemons;
using DOM.Project.Typings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.EF;

public class ProjectDbContext : DbContext
{
    public DbSet<Typing> Types { get; set; }
    public DbSet<TypingWeakness> TypeWeaknesses { get; set; }
    public DbSet<TypingResistance> TypeResistances { get; set; }
    public DbSet<TypingImmunities> TypeImmunities { get; set; }
    public DbSet<Ability> Abilities { get; set; }
    public DbSet<AbilityFlag> AbilityFlags { get; set; }
    public DbSet<Move> Moves { get; set; }
    public DbSet<MoveCategory> MoveCategories { get; set; }
    public DbSet<MoveFlag> MoveFlags { get; set; }
    public DbSet<MoveFunctionCode> MoveFunctionCodes { get; set; }
    public DbSet<MoveTarget> MoveTargets { get; set; }
    public DbSet<LearnedMove> LearnedMoves { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemFlag> ItemFlags { get; set; }
    public DbSet<ItemBattleUse> ItemBattleUses { get; set; }
    public DbSet<ItemFieldUse> ItemFieldUses { get; set; }
    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<PokemonColor> PokemonColors { get; set; }
    public DbSet<PokemonEggGroup> PokemonEggGroups { get; set; }
    public DbSet<PokemonEvolution> PokemonEvolutions { get; set; }
    public DbSet<PokemonEvolutionMethod> PokemonEvolutionMethods { get; set; }
    public DbSet<PokemonFlag> PokemonFlags { get; set; }
    public DbSet<PokemonGenderRatio> PokemonGenderRatios { get; set; }
    public DbSet<PokemonGrowthRate> PokemonGrowthRates { get; set; }
    public DbSet<PokemonHabitat> PokemonHabitats { get; set; }
    public DbSet<PokemonShape> PokemonShapes { get; set; }
    public DbSet<PokemonEvGained> PokemonEvGained { get; set; }

    
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
    {
        ProjectInitializer.Initialize(this, true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) {
            // configurations if no options are provided via the constructor
            // eg. Data provider and data source
            optionsBuilder.UseSqlite("Data Source=defaultProjectDatabase.db");
            
        }
        // Configurations that should always be applied
        // ...
        optionsBuilder.LogTo(message => Debug.WriteLine(message), LogLevel.Information);
        // Console.WriteLine(optionsBuilder.Options
        //     .FindExtension<Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal.SqliteOptionsExtension>()
        //     ?.ConnectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Types
        modelBuilder.Entity<Typing>()
            .HasMany(t => t.Resistances)
            .WithOne(tr => tr.Typing)
            .IsRequired();
        
        modelBuilder.Entity<Typing>()
            .HasMany(t => t.Weaknesses)
            .WithOne(tw => tw.Typing)
            .IsRequired();
        
        modelBuilder.Entity<Typing>()
            .HasMany(t => t.Immunities)
            .WithOne(tw => tw.Typing)
            .IsRequired();

        //Abilities
        modelBuilder.Entity<Ability>()
            .HasMany(a => a.Flags)
            .WithMany(af => af.Abilities);

        //Moves
        modelBuilder.Entity<Move>()
            .HasOne(m => m.Category)
            .WithMany(mc => mc.Moves)
            .IsRequired();

        modelBuilder.Entity<Move>()
            .HasMany(m => m.Flags)
            .WithMany(mf => mf.Moves);
        
        modelBuilder.Entity<Move>()
            .HasOne(m => m.FunctionCode)
            .WithMany(mfc => mfc.Moves);
        
        modelBuilder.Entity<Move>()
            .HasOne(m => m.Target)
            .WithMany(mt => mt.Moves)
            .IsRequired();
        
        //Items
        modelBuilder.Entity<Item>()
            .HasOne(i => i.BattleUse)
            .WithMany(ibu => ibu.Items);
        
        modelBuilder.Entity<Item>()
            .HasOne(i => i.FieldUse)
            .WithMany(ifu => ifu.Items);
        
        modelBuilder.Entity<Item>()
            .HasMany(i => i.Flags)
            .WithMany(f => f.Items);
        
        modelBuilder.Entity<Item>()
            .HasOne(i => i.Move);
        
        //Pokemons
        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.Moves)
            .WithOne(m => m.Pokemon);
        
        modelBuilder.Entity<Pokemon>()
            .HasOne(p => p.Color)
            .WithMany(c => c.Pokemons);
        
        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.EggGroups)
            .WithMany(c => c.Pokemons);
        
        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.Evolutions)
            .WithOne(c => c.PokemonBefore);
        
        modelBuilder.Entity<PokemonEvolution>()
            .HasOne(em => em.PokemonEvolutionMethod)
            .WithMany(c => c.Evolutions);
        
        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.Flags)
            .WithMany(c => c.Pokemons);
        
        modelBuilder.Entity<Pokemon>()
            .HasOne(p => p.GenderRatio)
            .WithMany(c => c.Pokemons);
        
        modelBuilder.Entity<Pokemon>()
            .HasOne(p => p.GrowthRate)
            .WithMany(c => c.Pokemons);
        
        modelBuilder.Entity<Pokemon>()
            .HasOne(p => p.Habitat)
            .WithMany(c => c.Pokemons);
        
        modelBuilder.Entity<Pokemon>()
            .HasOne(p => p.Shape)
            .WithMany(c => c.Pokemons);
        
        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.Offspring)
            .WithMany(o => o.Parents);
        
        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.EvsGained)
            .WithMany(c => c.Pokemons);
    }
}