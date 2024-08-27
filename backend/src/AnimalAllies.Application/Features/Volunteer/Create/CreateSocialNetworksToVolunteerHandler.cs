using AnimalAllies.Application.Features.Volunteer.Update;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Create;

public class CreateSocialNetworksToVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<CreateSocialNetworksToVolunteerHandler> _logger;

    public CreateSocialNetworksToVolunteerHandler(
        IVolunteerRepository repository,
        ILogger<CreateSocialNetworksToVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        CreateSocialNetworksRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _repository.GetById(VolunteerId.Create(request.Id), cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound();

        var socialNetworks = request.Dto.SocialNetworks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value);

        var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks);

        volunteer.Value.UpdateSocialNetworks(volunteerSocialNetworks);
        
        var result = await _repository.Update(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} updated social networks",  request.Id);

        return result;
    }
}