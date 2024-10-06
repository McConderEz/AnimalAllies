using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.Repository;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.MovePetPosition;

public class MovePetPositionHandler : ICommandHandler<MovePetPositionCommand, VolunteerId>
{
    private readonly IVolunteerRepository _repository;
    private readonly IValidator<MovePetPositionCommand> _validator;
    private readonly ILogger<MovePetPositionHandler> _logger;

    public MovePetPositionHandler(
        IVolunteerRepository repository,
        IValidator<MovePetPositionCommand> validator,
        ILogger<MovePetPositionHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<VolunteerId>> Handle(MovePetPositionCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteer = await _repository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Errors;

        var petId = PetId.Create(command.PetId);

        var pet = volunteer.Value.GetPetById(petId);
        if (pet.IsFailure)
            return pet.Errors;

        var position = Position.Create(command.Position.Position).Value;

        var moveResult = volunteer.Value.MovePet(pet.Value, position);

        if (moveResult.IsFailure)
            return moveResult.Errors;

        var result = await _repository.Save(volunteer.Value, cancellationToken);

        if (result.IsFailure)
            return result.Errors;

        _logger.LogInformation(
            "pet with id {petId} of volunteer with id {volunteerId} move to position {position}",
            petId.Id,
            volunteerId.Id,
            position);

        return volunteerId;
    }
    
}
