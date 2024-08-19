using AnimalAllies.Application.Repositories;
using AnimalAllies.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();

        return services;
    }
}