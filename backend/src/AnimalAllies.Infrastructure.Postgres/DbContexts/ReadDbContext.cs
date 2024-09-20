using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Database;
using AnimalAllies.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Infrastructure.DbContexts;

public class ReadDbContext(IConfiguration configuration):DbContext, IReadDbContext
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
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ReadDbContext).Assembly, 
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    private static readonly ILoggerFactory CreateLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });

    public IQueryable<VolunteerDto> Volunteers => Set<VolunteerDto>();
} 