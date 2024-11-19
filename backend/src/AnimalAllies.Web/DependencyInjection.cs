using AnimalAllies.Accounts.Application;
using AnimalAllies.Accounts.Infrastructure;
using AnimalAllies.Accounts.Presentation;
using AnimalAllies.Framework.Models;
using AnimalAllies.Species.Application;
using AnimalAllies.Species.Infrastructure;
using AnimalAllies.Species.Presentation;
using AnimalAllies.Volunteer.Application;
using AnimalAllies.Volunteer.Infrastructure;
using AnimalAllies.Volunteer.Presentation;
using Discussion.Application;
using Discussion.Infrastructure;
using Discussion.Presentation;
using VolunteerRequests.Application;
using VolunteerRequests.Infrastructure;

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
            .AddBreedsManagementModule(configuration)
            .AddVolunteerRequestsManagementModule(configuration)
            .AddDiscussionManagementModule(configuration)
            .AddFramework();

        return services;
    }
    
    private static IServiceCollection AddFramework(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<UserScopedData>();

        return services;
    }

    private static IServiceCollection AddVolunteerRequestsManagementModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddVolunteerRequestsInfrastructure(configuration)
            .AddVolunteerRequestsApplication();
        
        return services;
    }
    
    private static IServiceCollection AddDiscussionManagementModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDiscussionInfrastructure(configuration)
            .AddDiscussionApplication()
            .AddDiscussionPresentation();
        
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