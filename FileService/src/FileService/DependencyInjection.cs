using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Data.Options;
using FileService.Infrastructure;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;
using Minio;

namespace FileService;

public static class DependencyInjection
{
    public static IServiceCollection AddFileServiceInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMinio(configuration)
            .AddMongoDb(configuration)
            .AddRepositories();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFilesDataRepository,FilesDataRepository>();

        return services;
    }
    
    private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDatabaseSettings>(configuration.GetSection(MongoDatabaseSettings.Mongo));
        services.AddScoped<ApplicationDbContext>();
        
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