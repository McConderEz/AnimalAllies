using AnimalAllies.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Infrastructure;

public class AnimalAlliesDbContext: DbContext
{
    private readonly IConfiguration _configuration;
    
    public AnimalAlliesDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_configuration.GetConnectionString("DefaultConnection"))
            .UseLoggerFactory(CreateLoggerFactory)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnimalAlliesDbContext).Assembly);
    }

    public static readonly ILoggerFactory CreateLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<PetPhoto> PetPhotos { get; set; }
    public DbSet<Species> SpeciesList { get; set; }
}