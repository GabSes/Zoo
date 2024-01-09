using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using FluentValidation;
using Zoo;
using Zoo.Data;
using Zoo.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ZooDbContext>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();



// PostgreSQL
// Npgsql.EntityFrameworkCore.PostgreSQL
// Microsoft.EntityFrameworkCore.Tools

// FluentValidation
// FluentValidation.DependancyInjectionExtensions
// O9d.AspNet.FluentValidation

// dotnet tool install --global dotnet -ef
var app = builder.Build();


var enclosureGroup = app.MapGroup("/api").WithValidationFilter();
EnclosureEndpoints.AddEnclosureApi(enclosureGroup);
var animalGroup = app.MapGroup("/api/enclosures/{enclosureId}").WithValidationFilter();
AnimalEndpoints.AddAnimalApi(animalGroup);


app.Run();