using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using AnimalAllies.Volunteer.Application.Repository;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.UpdateVolunteer;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.ValueObject;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CreateSocialNetworks;

public class CreateSocialNetworksHandler : ICommandHandler<CreateSocialNetworksCommand, VolunteerId>
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