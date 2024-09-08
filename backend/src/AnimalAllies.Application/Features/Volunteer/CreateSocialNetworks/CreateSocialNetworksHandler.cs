using AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.CreateSocialNetworks;

public class CreateSocialNetworksHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;

    public CreateSocialNetworksHandler(
        IVolunteerRepository repository,
        ILogger<UpdateVolunteerHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        CreateSocialNetworksCommand request,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _repository.GetById(VolunteerId.Create(request.Id), cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound();

        var socialNetworks = request.SocialNetworks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value);

        var volunteerSocialNetworks = new VolunteerSocialNetworks(socialNetworks.ToList());

        volunteer.Value.UpdateSocialNetworks(volunteerSocialNetworks);
        
        var result = await _repository.Save(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} updated social networks",  request.Id);

        return result;
    }
}