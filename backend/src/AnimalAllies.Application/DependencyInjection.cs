using AnimalAllies.Application.Features.Volunteer;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPet;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPetPhoto;
using AnimalAllies.Application.Features.Volunteer.Commands.CreateRequisites;
using AnimalAllies.Application.Features.Volunteer.Commands.CreateSocialNetworks;
using AnimalAllies.Application.Features.Volunteer.Commands.CreateVolunteer;
using AnimalAllies.Application.Features.Volunteer.Commands.DeleteVolunteer;
using AnimalAllies.Application.Features.Volunteer.Commands.MovePetPosition;
using AnimalAllies.Application.Features.Volunteer.Commands.UpdateVolunteer;
using AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;
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
        services.AddScoped<MovePetPositionHandler>();
        services.AddScoped<GetVolunteersWithPaginationHandler>();
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}