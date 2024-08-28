using AnimalAllies.Application.Features.Volunteer;
using AnimalAllies.Application.Features.Volunteer.Create;
using AnimalAllies.Application.Features.Volunteer.CreateRequisites;
using AnimalAllies.Application.Features.Volunteer.CreateSocialNetworks;
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
        services.AddScoped<CreateRequisitesHandler>();
        services.AddScoped<CreateSocialNetworksHandler>();
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}