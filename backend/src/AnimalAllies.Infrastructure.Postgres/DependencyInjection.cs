using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models.Common;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Infrastructure.Common;
using AnimalAllies.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<AnimalAlliesDbContext>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        
        return services;
    }
}