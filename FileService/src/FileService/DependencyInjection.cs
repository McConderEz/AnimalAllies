using Amazon.S3;
using FileService.Api.Extensions;
using FileService.Application.Providers;
using FileService.Application.Repositories;
using FileService.Data.Options;
using FileService.Infrastructure.Providers;
using FileService.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Minio;
using MongoDB.Driver;
using MongoDatabaseSettings = FileService.Data.Options.MongoDatabaseSettings;

namespace FileService;

public static class DependencyInjection
{
    public static IServiceCollection AddFileServiceInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMinio(configuration)
            .AddMongo(configuration)
            .AddRepositories();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFilesDataRepository,FilesDataRepository>();

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
        
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO)
                .Get<MinioOptions>() ?? throw new ApplicationException("Missing minio configuration");
            
            var config = new AmazonS3Config
            {
                ServiceURL = minioOptions.AwsEndpoint,
                ForcePathStyle = true,
                UseHttp = true
            };

            return new AmazonS3Client(minioOptions.AccessKey, minioOptions.SecretKey, config);
        });


        services.AddScoped<IFileProvider, MinioProvider>();

        services.AddTransient<Seeding>();

        return services;
    }
    
    private static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDatabaseSettings>(
            configuration.GetSection(MongoDatabaseSettings.Mongo) ?? throw new ApplicationException());
        
        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });
        
        services.AddScoped<IMongoDatabase>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value;
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });

        return services;
    }
}