using Microsoft.EntityFrameworkCore;

namespace Nutrino.Web.Data;

public enum ServingUnit { Gram, Milliliter, Ounce, Cup, Tablespoon, Teaspoon, Piece }
public enum MealType { Breakfast, Lunch, Dinner, Snack }

// A food the user defines once. Macros are for ONE serving.
public class Food
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? Brand { get; set; }
    public decimal ServingSize { get; set; }        // e.g. 170
    public ServingUnit ServingUnit { get; set; }    // e.g. Gram
    public decimal Calories { get; set; }
    public decimal ProteinG { get; set; }
    public decimal CarbsG { get; set; }
    public decimal FatG { get; set; }
    public decimal? FiberG { get; set; }            // optional extras
    public decimal? SugarG { get; set; }
    public decimal? SodiumMg { get; set; }
    public bool IsArchived { get; set; }            // hide instead of delete
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<FoodLogEntry> LogEntries { get; set; } = [];
}

// One "I ate this" record.
public class FoodLogEntry
{
    public int Id { get; set; }
    public int FoodId { get; set; }
    public Food Food { get; set; } = null!;
    public DateOnly Date { get; set; }
    public MealType Meal { get; set; }
    public decimal Servings { get; set; } = 1;      // 1.5 = one and a half servings

    // Totals copied when logged (Food macros x Servings)
    public decimal Calories { get; set; }
    public decimal ProteinG { get; set; }
    public decimal CarbsG { get; set; }
    public decimal FatG { get; set; }
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
}

// The targets shown on the home page.
public class DailyGoal
{
    public int Id { get; set; }
    public decimal Calories { get; set; }
    public decimal ProteinG { get; set; }
    public decimal CarbsG { get; set; }
    public decimal FatG { get; set; }
}

public class NutrinoDbContext(DbContextOptions<NutrinoDbContext> options) : DbContext(options)
{
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<FoodLogEntry> FoodLogEntries => Set<FoodLogEntry>();
    public DbSet<DailyGoal> DailyGoals => Set<DailyGoal>();

    protected override void ConfigureConventions(ModelConfigurationBuilder b)
    {
        // All macro numbers: up to 999,999.99
        b.Properties<decimal>().HavePrecision(8, 2);
    }

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Food>(e =>
        {
            e.Property(f => f.Name).HasMaxLength(120);
            e.Property(f => f.Brand).HasMaxLength(120);
            e.Property(f => f.ServingUnit).HasConversion<string>().HasMaxLength(20);
            e.HasIndex(f => f.Name);
        });

        m.Entity<FoodLogEntry>(e =>
        {
            e.Property(l => l.Meal).HasConversion<string>().HasMaxLength(20);
            e.HasIndex(l => l.Date);   // fast "what did I eat today" lookups
            e.HasOne(l => l.Food)
             .WithMany(f => f.LogEntries)
             .HasForeignKey(l => l.FoodId)
             .OnDelete(DeleteBehavior.Restrict);   // can't delete a food that's been logged
        });
    }
}