using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.MovePetPosition;

public class MovePetPositionHandler
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

        var moveResult = volunteer.Value.MovePet(pet.Value, command.Position);

        if (moveResult.IsFailure)
            return moveResult.Errors;

        var result = await _repository.Save(volunteer.Value, cancellationToken);

        if (result.IsFailure)
            return result.Errors;

        _logger.LogInformation(
            "pet with id {petId} of volunteer with id {volunteerId} move to position {position}",
            petId.Id,
            volunteerId.Id,
            command.Position.Value);

        return volunteerId;
    }
    
}
