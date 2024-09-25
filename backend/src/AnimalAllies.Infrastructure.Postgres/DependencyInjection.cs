using AnimalAllies.Application.Database;
using AnimalAllies.Application.Messaging;
using AnimalAllies.Application.Providers;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Infrastructure.BackgroundServices;
using AnimalAllies.Infrastructure.Common;
using AnimalAllies.Infrastructure.DbContexts;
using AnimalAllies.Infrastructure.MessageQueues;
using AnimalAllies.Infrastructure.Options;
using AnimalAllies.Infrastructure.Providers;
using AnimalAllies.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using FileInfo = AnimalAllies.Application.FileProvider.FileInfo;
using ServiceCollectionExtensions = Minio.ServiceCollectionExtensions;

namespace AnimalAllies.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts()
            .AddMinio(configuration)
            .AddRepositories()
            .AddDatabase()
            .AddHostedServices();
        
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>,InMemoryMessageQueue<IEnumerable<FileInfo>>>();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<FilesCleanerBackgroundService>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<ISqlConnectionFactory,SqlConnectionFactory>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext,ReadDbContext>();

        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));
        
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(Options.MinioOptions.MINIO)
                .Get<Options.MinioOptions>() ?? throw new ApplicationException("Missing minio configuration");
                
            
            options.WithEndpoint(minioOptions.Endpoint);

            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

            options.WithSSL(minioOptions.WithSsl);

        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }
}