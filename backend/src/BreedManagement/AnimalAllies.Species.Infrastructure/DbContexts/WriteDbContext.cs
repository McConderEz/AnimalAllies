using AnimalAllies.Core.Interceptors;
using AnimalAllies.SharedKernel.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Species.Infrastructure.DbContexts;

public class WriteDbContext(IConfiguration configuration) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseLoggerFactory(CreateLoggerFactory)
            .EnableSensitiveDataLogging()
            .UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("species");
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly, 
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

    private static readonly ILoggerFactory CreateLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });
    
    public DbSet<Domain.Species> Species { get; set; } = null!;
}