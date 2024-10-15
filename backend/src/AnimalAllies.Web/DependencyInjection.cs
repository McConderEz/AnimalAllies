using AnimalAllies.Accounts.Application;
using AnimalAllies.Accounts.Infrastructure;
using AnimalAllies.Accounts.Presentation;
using AnimalAllies.Species.Application;
using AnimalAllies.Species.Infrastructure;
using AnimalAllies.Species.Presentation;
using AnimalAllies.Volunteer.Application;
using AnimalAllies.Volunteer.Infrastructure;
using AnimalAllies.Volunteer.Presentation;

namespace AnimalAllies.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddModules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAccountsManagementModule(configuration)
            .AddPetsManagementModule(configuration)
            .AddBreedsManagementModule(configuration);

        return services;
    }

    private static IServiceCollection AddAccountsManagementModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAccountsPresentation()
            .AddAccountsApplication()
            .AddAccountsInfrastructure(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddPetsManagementModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddVolunteerPresentation()
            .AddVolunteerApplication()
            .AddVolunteerInfrastructure(configuration);

        return services;
    }
    
    private static IServiceCollection AddBreedsManagementModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSpeciesPresentation()
            .AddSpeciesApplication()
            .AddSpeciesInfrastructure(configuration);

        return services;
    }
}