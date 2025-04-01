using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileService.Communication;

public static class FileServiceExtensions
{
    public static IServiceCollection AddFileHttpCommunication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileServiceOptions>(configuration.GetSection(FileServiceOptions.FILE_SERVICE));
        
        services.AddHttpClient<FileHttpClient>((sp, config) =>
        {
            var options = sp.GetRequiredService<IOptions<FileServiceOptions>>().Value;
            
            config.BaseAddress = new Uri(options.Url);
        });

        return services;
    }
}