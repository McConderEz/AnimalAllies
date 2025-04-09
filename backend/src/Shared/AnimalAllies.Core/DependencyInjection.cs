using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddQuartz();

        return services;
    }

    private static IServiceCollection AddQuartz(this IServiceCollection services)
    {
        
        return services;
    }
}