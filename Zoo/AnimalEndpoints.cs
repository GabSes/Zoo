using Microsoft.EntityFrameworkCore;
using O9d.AspNet.FluentValidation;
using Zoo.Data;
using Zoo.Data.Dtos;
using Zoo.Data.Entities;

namespace Zoo;


public static class AnimalEndpoints
{
    public static void AddAnimalApi(RouteGroupBuilder animalGroup)
    {
        animalGroup.MapGet("animals", async (int enclosureId, ZooDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var animals = await dbContext.Animals
                .Where(animal => animal.Enclosure.Id == enclosureId)
                .ToListAsync(cancellationToken);
            return Results.Ok(animals);
        });

        animalGroup.MapGet("animals/{animalId}", async (int enclosureId, int animalId, ZooDbContext dbContext) =>
        {
            var animal = await dbContext.Animals
                .FirstOrDefaultAsync(t => t.Id == animalId && t.Enclosure.Id == enclosureId);
            if (animal == null)
                return Results.NotFound();
            return Results.Ok(animal);
        });

        animalGroup.MapPost("animals", async (int enclosureId, [Validate] CreateAnimalDto createAnimalDto, ZooDbContext dbContext) =>
        {
            var animal = new Animal
            {
                Species = createAnimalDto.Species,
                Food = createAnimalDto.Food,
                Amount = createAnimalDto.Amount,
                EnclosureId = enclosureId  // Assigning the specified enclosure
            };

            dbContext.Animals.Add(animal);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/api/enclosure/{enclosureId}/animals/{animal.Id}", animal);
        });

        animalGroup.MapPut("animals/{animalId}", async (int enclosureId, int animalId, [Validate] UpdateAnimalDto animalDto, ZooDbContext dbContext) =>
        {
            var animal = await dbContext.Animals
                .FirstOrDefaultAsync(t => t.Id == animalId && t.Enclosure.Id == enclosureId);
            if (animal == null)
                return Results.NotFound();

            animal.Species = animalDto.Species;
            animal.Food = animalDto.Food;
            animal.Amount = animalDto.Amount;
            dbContext.Update(animal);
            await dbContext.SaveChangesAsync();
            return Results.Ok(animal);
        });

        animalGroup.MapDelete("animals/{animalId}", async (int enclosureId, int animalId, ZooDbContext dbContext) =>
        {
            var animal = await dbContext.Animals
                .FirstOrDefaultAsync(t => t.Id == animalId && t.Enclosure.Id == enclosureId);
            if (animal == null)
                return Results.NotFound();
            dbContext.Remove(animal);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });
        animalGroup.MapPost("animals/accommodate", async (ZooDbContext dbContext) =>
            {
                var enclosures = await dbContext.Enclosures.Include(e => e.Animals).ToListAsync();
                var animals = await dbContext.Animals.ToListAsync();

                foreach (var animal in animals)
                {
                    var suitableEnclosure = FindSuitableEnclosure(enclosures, animal);

                    if (suitableEnclosure != null)
                    {
                        animal.EnclosureId = suitableEnclosure.Id;
                        suitableEnclosure.Animals.Add(animal);
                    }
                    else
                    {
                        // Handle the case where no suitable enclosure is found (create a new enclosure)
                        var newEnclosure = new Enclosure
                        {
                            // Initialize new enclosure properties 
                            Name = $"New Enclosure {Guid.NewGuid().ToString().Substring(0, 8)}",
                            Size = "Medium",  // Set a default size 
                            Location = "Outside",  // Set a default location 
                            Objects = new string[0], 
                            AllowedSpecies = animal.Food.ToLower() == "herbivore"
                                ? new[] { animal.Species }  // Vegetarian animals can be placed together
                                : new string[0]  // for meat-eating animals
                        };

                        dbContext.Enclosures.Add(newEnclosure);
                        await dbContext.SaveChangesAsync();

                        animal.EnclosureId = newEnclosure.Id;
                        newEnclosure.Animals.Add(animal);
                    }
                }

                await dbContext.SaveChangesAsync();
                return Results.Ok("Accommodation process completed successfully");
            });
        }
    private static Enclosure FindSuitableEnclosure(List<Enclosure> enclosures, Animal animal)
    {
        foreach (var enclosure in enclosures)
        {
            if (enclosure.AllowedSpecies.Contains(animal.Species))
            {
                if (enclosure.Animals.Any())
                {
                    if (enclosure.Animals.Any(a => a.Species == animal.Species))
                    {
                        continue;
                    }

                    if (animal.Food.ToLower() == "carnivore" && enclosure.Animals.Any(a => a.Food.ToLower() == "carnivore"))
                    {
                        if (enclosure.Animals.Select(a => a.Species).Distinct().Count() > 1)
                        {
                            continue;
                        }
                    }
                }

                if (enclosure.Animals.Count < MaxAnimalsPerEnclosure)
                {
                    return enclosure;
                }
            }
        }

        return null;
    }

    private const int MaxAnimalsPerEnclosure = 5;
}


