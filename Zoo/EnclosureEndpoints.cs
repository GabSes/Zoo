using Microsoft.EntityFrameworkCore;
using O9d.AspNet.FluentValidation;
using Zoo.Data;
using Zoo.Data.Dtos;
using Zoo.Data.Entities;

namespace Zoo;

public static class EnclosureEndpoints
{
    public static void AddEnclosureApi(RouteGroupBuilder enclosureGroup)
    {
        enclosureGroup.MapGet("enclosures", async (ZooDbContext dbContext, CancellationToken cancellationToken) =>
        {
            var enclosures = await dbContext.Enclosures.ToListAsync(cancellationToken);
            return Results.Ok(enclosures);
        });

        enclosureGroup.MapGet("enclosures/{enclosureId}", async (int enclosureId, ZooDbContext dbContext) =>
        {
            var enclosure = await dbContext.Enclosures.FindAsync(enclosureId);
            if (enclosure == null)
                return Results.NotFound();
            return Results.Ok(enclosure);
        });

        enclosureGroup.MapPost("enclosures", async ([Validate] CreateEnclosureDto createEnclosureDto, ZooDbContext dbContext) =>
        {
            var enclosure = new Enclosure
            {
                Name = createEnclosureDto.Name,
                Size = createEnclosureDto.Size,
                Location = createEnclosureDto.Location,
                Objects = createEnclosureDto.Objects
            };
            dbContext.Enclosures.Add(enclosure);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/api/enclosures/{enclosure.Id}", enclosure);
        });

        enclosureGroup.MapPut("enclosures/{enclosureId}", async (int enclosureId, [Validate] UpdateEnclosureDto enclosureDto, ZooDbContext dbContext) =>
        {
            var enclosure = await dbContext.Enclosures.FindAsync(enclosureId);
            if (enclosure == null)
                return Results.NotFound();

            enclosure.Name = enclosureDto.Name;
            enclosure.Size = enclosureDto.Size;
            enclosure.Location = enclosureDto.Location;
            enclosure.Objects = enclosureDto.Objects;

            dbContext.Update(enclosure);
            await dbContext.SaveChangesAsync();
            return Results.Ok(enclosure);
        });

        enclosureGroup.MapDelete("enclosures/{enclosureId}", async (int enclosureId, ZooDbContext dbContext) =>
        {
            var enclosure = await dbContext.Enclosures.FindAsync(enclosureId);
            if (enclosure == null)
                return Results.NotFound();

            dbContext.Remove(enclosure);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });
         enclosureGroup.MapPost("enclosures/accommodate", async (ZooDbContext dbContext) =>
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
                            // Initialize new enclosure properties based on your requirements
                            Name = $"New Enclosure {Guid.NewGuid().ToString().Substring(0, 8)}",
                            Size = "Medium",  // Set a default size or determine dynamically
                            Location = "Outside",  // Set a default location or determine dynamically
                            Objects = new string[0],  // Initialize based on your requirements for objects
                            AllowedSpecies = animal.Food.ToLower() == "herbivore"
                                ? new[] { animal.Species }  // Vegetarian animals can be placed together
                                : new string[0]  // Initialize based on your requirements for meat-eating animals
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

    private const int MaxAnimalsPerEnclosure = 5; // Adjust this value based on your requirements
}
    

