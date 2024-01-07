using Microsoft.EntityFrameworkCore;
using Zoo.Data.Entities;

namespace Zoo.Data;

public class ZooDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Enclosure> Enclosures { get; set; }

    public ZooDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
    }
}