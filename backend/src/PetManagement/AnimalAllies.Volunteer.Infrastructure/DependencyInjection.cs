using AnimalAllies.Core.Common;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Messaging;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.Volunteer.Application.Providers;
using AnimalAllies.Volunteer.Application.Repository;
using AnimalAllies.Volunteer.Infrastructure.BackgroundServices;
using AnimalAllies.Volunteer.Infrastructure.DbContexts;
using AnimalAllies.Volunteer.Infrastructure.MessageQueues;
using AnimalAllies.Volunteer.Infrastructure.Options;
using AnimalAllies.Volunteer.Infrastructure.Providers;
using AnimalAllies.Volunteer.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace AnimalAllies.Volunteer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts()
            .AddMinio(configuration)
            .AddRepositories()
            .AddDatabase()
            .AddHostedServices();
        
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        
        services.AddSingleton<IMessageQueue<IEnumerable<Application.FileProvider.FileInfo>>,
            InMemoryMessageQueue<IEnumerable<Application.FileProvider.FileInfo>>>();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
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
        services.AddScoped<IReadDbContext, ReadDbContext>();

        return services;
    }

    private static IServiceCollection AddMinio(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.MINIO));
        
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO)
                .Get<MinioOptions>() ?? throw new ApplicationException("Missing minio configuration");
                
            
            options.WithEndpoint(minioOptions.Endpoint);

            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

            options.WithSSL(minioOptions.WithSsl);

        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }
}