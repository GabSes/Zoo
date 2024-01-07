using Microsoft.AspNetCore.Mvc;
using Zoo.Data;
using Zoo.Data.Dtos;
using Zoo.Data.Entities;
using System.Linq; // Add this using statement

[ApiController]
[Route("api/zoo")]
public class ZooController : ControllerBase
{
    private readonly ZooDbContext _dbContext;

    public ZooController(ZooDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("seed-data")]
    public IActionResult SeedData([FromBody] ZooDataDto zooData)
    {
        try
        {
            // Convert EnclosureDto to Enclosure
            var enclosures = zooData.Enclosures.Select(dto => new Enclosure
            {
                Name = dto.Name,
                Size = dto.Size,
                Location = dto.Location,
                Objects = dto.Objects,
                AllowedSpecies = dto.AllowedSpecies
            }).ToList();

            // Seed Enclosures
            _dbContext.Enclosures.AddRange(enclosures);

            // Convert AnimalDto to Animal
            var animals = zooData.Animals.Select(dto => new Animal
            {
                Species = dto.Species,
                Food = dto.Food,
                Amount = dto.Amount,
                EnclosureId = dto.EnclosureId
            }).ToList();

            // Seed Animals
            _dbContext.Animals.AddRange(animals);

            _dbContext.SaveChanges();

            return Ok("Data seeded successfully!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error seeding data: {ex.Message}");
        }
    }
}

public class JsonFileDto
{
    public string Content { get; set; }
}