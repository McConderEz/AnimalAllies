using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Commands.UpdatePetStatus;

public class UpdatePetStatusHandler : ICommandHandler<UpdatePetStatusCommand, PetId>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePetStatusHandler> _logger;
    private readonly IValidator<UpdatePetStatusCommand> _validator;

    public UpdatePetStatusHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdatePetStatusHandler> logger,
        IDateTimeProvider dateTimeProvider,
        IValidator<UpdatePetStatusCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task<Result<PetId>> Handle(UpdatePetStatusCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteer = await _volunteerRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Errors;

        var petId = PetId.Create(command.PetId);

        var helpStatus = HelpStatus.Create(command.HelpStatus);
        if (helpStatus.IsFailure)
            return helpStatus.Errors;

        var result = volunteer.Value.UpdatePetStatus(petId, helpStatus.Value);
        if (result.IsFailure)
            return result.Errors;
        
        _logger.LogInformation("Update status to pet with id {petId} from volunteer with id {volunteerId}",
            petId.Id, volunteerId.Id);

        await _unitOfWork.SaveChanges(cancellationToken);

        return petId;
    }
}