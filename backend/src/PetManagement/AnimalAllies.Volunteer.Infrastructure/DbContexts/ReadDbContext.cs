using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Infrastructure.DbContexts;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseLoggerFactory(CreateLoggerFactory)
            .EnableSensitiveDataLogging()
            .UseSnakeCaseNamingConvention();

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("volunteers");
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    private static readonly ILoggerFactory CreateLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });

    public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
    public IQueryable<BreedDto> Breeds => Set<BreedDto>();
    public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();
    public IQueryable<PetDto> Pets => Set<PetDto>();
}