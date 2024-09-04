using AnimalAllies.Application.Features.Volunteer;
using AnimalAllies.Application.Features.Volunteer.AddPet;
using AnimalAllies.Application.Features.Volunteer.AddPetPhoto;
using AnimalAllies.Application.Features.Volunteer.CreateRequisites;
using AnimalAllies.Application.Features.Volunteer.CreateSocialNetworks;
using AnimalAllies.Application.Features.Volunteer.CreateVolunteer;
using AnimalAllies.Application.Features.Volunteer.DeleteVolunteer;
using AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;
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
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<AddPetHandler>();
        services.AddScoped<AddPetPhotosHandler>();
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}