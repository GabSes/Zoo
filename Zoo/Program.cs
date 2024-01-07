using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Zoo.Data;
using Zoo.Data.Entities;
using Zoo.Services;  // Add this using statement

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ZooDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

        builder.Services.AddTransient<ZooDataSeeder>();  // Register the ZooDataSeeder service

        builder.Services.AddControllers();

        var app = builder.Build();

        // Seed initial data during application startup
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var zooDataSeeder = services.GetRequiredService<ZooDataSeeder>();
            zooDataSeeder.SeedDataFromJson();  // Add this method in your ZooDataSeeder class
        }

        app.MapControllers();

        app.Run();
    }
}