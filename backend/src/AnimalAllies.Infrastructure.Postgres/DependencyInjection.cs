using AnimalAllies.Application.Providers;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models.Common;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Infrastructure.Common;
using AnimalAllies.Infrastructure.Options;
using AnimalAllies.Infrastructure.Providers;
using AnimalAllies.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using ServiceCollectionExtensions = Minio.ServiceCollectionExtensions;

namespace AnimalAllies.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<AnimalAlliesDbContext>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();

        services.AddMinio(configuration);
        
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