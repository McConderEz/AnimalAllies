using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.Repository;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.UpdatePetStatus;

public class UpdatePetStatusHandler : ICommandHandler<UpdatePetStatusCommand, PetId>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePetStatusHandler> _logger;
    private readonly IValidator<UpdatePetStatusCommand> _validator;

    public UpdatePetStatusHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdatePetStatusHandler> logger,
        IValidator<UpdatePetStatusCommand> validator,
        [FromKeyedServices(Constraints.Context.PetManagement)]IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
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