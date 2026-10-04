using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nutrino.Web.Data;

namespace Nutrino.Web.Controllers;

// Pages for creating, editing and archiving foods: /Foods
public class FoodsController(NutrinoDbContext db) : Controller
{
    // Only these fields can be filled in from the form (protects Id, IsArchived, CreatedAt).
    private const string FormFields =
        "Name,Brand,ServingSize,ServingUnit,Calories,ProteinG,CarbsG,FatG,FiberG,SugarG,SodiumMg";

    // GET /Foods   (/Foods?showArchived=true also lists hidden foods)
    public async Task<IActionResult> Index(bool showArchived = false)
    {
        var foods = await db.Foods
            .Where(f => showArchived || !f.IsArchived)
            .OrderBy(f => f.Name)
            .ToListAsync();

        ViewBag.ShowArchived = showArchived;
        return View(foods);
    }

    // GET /Foods/Create
    public IActionResult Create() =>
        View(new Food { ServingSize = 1, ServingUnit = ServingUnit.Piece });

    // POST /Foods/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(FormFields)] Food food)
    {
        CheckNumbers(food);
        if (!ModelState.IsValid) return View(food);

        food.CreatedAt = DateTime.UtcNow;
        db.Foods.Add(food);
        await db.SaveChangesAsync();

        TempData["Message"] = $"Added {food.Name}.";
        return RedirectToAction(nameof(Index));
    }

    // GET /Foods/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var food = await db.Foods.FindAsync(id);
        return food is null ? NotFound() : View(food);
    }

    // POST /Foods/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(FormFields)] Food input)
    {
        var food = await db.Foods.FindAsync(id);
        if (food is null) return NotFound();

        CheckNumbers(input);
        if (!ModelState.IsValid)
        {
            input.Id = id;
            return View(input);
        }

        food.Name = input.Name;
        food.Brand = input.Brand;
        food.ServingSize = input.ServingSize;
        food.ServingUnit = input.ServingUnit;
        food.Calories = input.Calories;
        food.ProteinG = input.ProteinG;
        food.CarbsG = input.CarbsG;
        food.FatG = input.FatG;
        food.FiberG = input.FiberG;
        food.SugarG = input.SugarG;
        food.SodiumMg = input.SodiumMg;
        await db.SaveChangesAsync();

        // Past log entries keep their own copied totals, so editing a food never changes history.
        TempData["Message"] = $"Saved {food.Name}.";
        return RedirectToAction(nameof(Index));
    }

    // POST /Foods/Archive/5 — hides the food instead of deleting it.
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Archive(int id)
    {
        var food = await db.Foods.FindAsync(id);
        if (food is null) return NotFound();

        food.IsArchived = true;
        await db.SaveChangesAsync();

        TempData["Message"] = $"Archived {food.Name}.";
        return RedirectToAction(nameof(Index));
    }

    // POST /Foods/Restore/5 — brings an archived food back.
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        var food = await db.Foods.FindAsync(id);
        if (food is null) return NotFound();

        food.IsArchived = false;
        await db.SaveChangesAsync();

        TempData["Message"] = $"Restored {food.Name}.";
        return RedirectToAction(nameof(Index), new { showArchived = true });
    }

    private void CheckNumbers(Food f)
    {
        if (f.ServingSize <= 0)
            ModelState.AddModelError(nameof(f.ServingSize), "Serving size must be more than 0.");

        var values = new (string Field, decimal? Value)[]
        {
            (nameof(f.Calories), f.Calories), (nameof(f.ProteinG), f.ProteinG),
            (nameof(f.CarbsG), f.CarbsG),     (nameof(f.FatG), f.FatG),
            (nameof(f.FiberG), f.FiberG),     (nameof(f.SugarG), f.SugarG),
            (nameof(f.SodiumMg), f.SodiumMg),
        };
        foreach (var (field, value) in values)
            if (value < 0) ModelState.AddModelError(field, "Can't be negative.");
    }
}
