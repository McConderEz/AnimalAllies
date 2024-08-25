using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer;

public class CreateSocialNetworksToVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public CreateSocialNetworksToVolunteerHandler(
        IVolunteerRepository repository,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        CreateSocialNetworksRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _repository.GetById(VolunteerId.Create(request.Id));

        if (volunteer.IsFailure)
            return Errors.General.NotFound();

        var socialNetworks = request.SocialNetworks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value);

        var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks);
        
        _logger.LogInformation("volunteer with id {volunteerId} updated social networks",  request.Id);
        
        return await _repository.AddSocialNetworks(VolunteerId.Create(request.Id), volunteerSocialNetworks, cancellationToken);
    }
}