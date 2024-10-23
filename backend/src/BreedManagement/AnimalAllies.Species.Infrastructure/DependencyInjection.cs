using AnimalAllies.Core.Database;
using AnimalAllies.Core.Options;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.Species.Application.Database;
using AnimalAllies.Species.Application.Repository;
using AnimalAllies.Species.Infrastructure.Backgroundservices;
using AnimalAllies.Species.Infrastructure.DbContexts;
using AnimalAllies.Species.Infrastructure.Repository;
using AnimalAllies.Species.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Species.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase()
            .AddDbContexts()
            .AddRepositories()
            .AddServices(configuration)
            .AddHostedServices();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EntityDeletion>(configuration.GetSection(EntityDeletion.ENTITY_DELETION));

        services.AddScoped<DeleteExpiredBreedsService>();
        services.AddScoped<DeleteExpiredSpeciesService>();

        return services;
    }
    
    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<EntityCleanerIfDeletedBackgroundService>();

        return services;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Constraints.Context.BreedManagement);
        services.AddKeyedSingleton<ISqlConnectionFactory,SqlConnectionFactory>(Constraints.Context.BreedManagement);

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext,ReadDbContext>();

        return services;
    }
    
}