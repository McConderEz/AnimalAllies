using AnimalAllies.Application.Features.Volunteer;
using AnimalAllies.Application.Features.Volunteer.Create;
using AnimalAllies.Application.Features.Volunteer.Delete;
using AnimalAllies.Application.Features.Volunteer.Update;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace AnimalAllies.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateVolunteerHandler>();
        services.AddScoped<CreateRequisitesToVolunteerHandler>();
        services.AddScoped<CreateSocialNetworksToVolunteerHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}