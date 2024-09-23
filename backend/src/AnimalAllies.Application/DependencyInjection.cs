using AnimalAllies.Application.Abstractions;
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
        services
            .AddCommands()
            .AddQueries()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);;
        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny([typeof(ICommandHandler<,>), typeof(ICommandHandler<>)]))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
}