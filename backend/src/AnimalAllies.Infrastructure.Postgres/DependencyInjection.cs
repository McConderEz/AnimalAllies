using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Common;
using AnimalAllies.Application.Services;
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

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerService, VolunteerService>();

        return services;
    }
}