using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Common;
using AnimalAllies.Application.Features.Volunteer;
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