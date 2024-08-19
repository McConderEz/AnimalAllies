using AnimalAllies.Application.Features.Volunteer;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalAllies.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        
        return services;
    }
}