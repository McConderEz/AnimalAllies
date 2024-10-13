using AnimalAllies.Core.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Infrastructure.DbContexts;

public class WriteDbContext(IConfiguration configuration) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            .UseLoggerFactory(CreateLoggerFactory)
            .EnableSensitiveDataLogging()
            .UseSnakeCaseNamingConvention();

        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("volunteers");
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly, 
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

    private static readonly ILoggerFactory CreateLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });

    public DbSet<Volunteer.Domain.VolunteerManagement.Aggregate.Volunteer> Volunteers { get; set; } = null!;
}