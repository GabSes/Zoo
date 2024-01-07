using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json;
using Zoo.Data;
using Zoo.Data.Dtos;
using Zoo.Data.Entities;

namespace Zoo.Services
{
    public class ZooDataSeeder
    {
        private readonly ZooDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public ZooDataSeeder(ZooDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public void SeedDataFromJson()
        {
            // Load and seed data from JSON files in the same directory as the application
            var animalsJson = File.ReadAllText("animals.json");
            var enclosuresJson = File.ReadAllText("enclosures.json");

            var zooData = new ZooDataDto
            {
                Animals = JsonSerializer.Deserialize<List<AnimalDto>>(animalsJson),
                Enclosures = JsonSerializer.Deserialize<List<EnclosureDto>>(enclosuresJson)
            };

            // Convert and seed Enclosures
            var enclosures = zooData.Enclosures.Select(dto => new Enclosure
            {
                Name = dto.Name,
                Size = dto.Size,
                Location = dto.Location,
                Objects = dto.Objects,
                AllowedSpecies = dto.AllowedSpecies
            }).ToList();
            _dbContext.Enclosures.AddRange(enclosures);

            // Convert and seed Animals
            var animals = zooData.Animals.Select(dto => new Animal
            {
                Species = dto.Species,
                Food = dto.Food,
                Amount = dto.Amount,
                EnclosureId = dto.EnclosureId
            }).ToList();
            _dbContext.Animals.AddRange(animals);

            _dbContext.SaveChanges();
        }
    }
}