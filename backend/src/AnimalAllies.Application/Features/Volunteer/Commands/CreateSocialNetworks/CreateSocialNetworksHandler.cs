using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Features.Volunteer.Commands.UpdateVolunteer;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Commands.CreateSocialNetworks;

public class CreateSocialNetworksHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly ILogger<UpdateVolunteerHandler> _logger;
    private readonly IValidator<CreateSocialNetworksCommand> _validator;
    
    public CreateSocialNetworksHandler(
        IVolunteerRepository repository,
        ILogger<UpdateVolunteerHandler> logger,
        IValidator<CreateSocialNetworksCommand> validator)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<VolunteerId>> Handle(
        CreateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var volunteer = await _repository.GetById(VolunteerId.Create(command.Id), cancellationToken);

        if (volunteer.IsFailure)
            return Errors.General.NotFound();

        var socialNetworks = command.SocialNetworks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value);

        var volunteerSocialNetworks = new ValueObjectList<SocialNetwork>(socialNetworks.ToList());

        volunteer.Value.UpdateSocialNetworks(volunteerSocialNetworks);
        
        var result = await _repository.Save(volunteer.Value, cancellationToken);
        
        _logger.LogInformation("volunteer with id {volunteerId} updated social networks",  command.Id);

        return result;
    }
}