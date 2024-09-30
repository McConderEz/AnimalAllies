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

namespace AnimalAllies.Application.Features.Volunteer.Commands.DeletePetSoft;

public class DeletePetSoftHandler: ICommandHandler<DeletePetSoftCommand, PetId>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePetSoftHandler> _logger;
    private readonly IValidator<DeletePetSoftCommand> _validator;

    public DeletePetSoftHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeletePetSoftHandler> logger,
        IValidator<DeletePetSoftCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }
    
    
    public async Task<Result<PetId>> Handle(DeletePetSoftCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteer = await _volunteerRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Errors;

        var petId = PetId.Create(command.PetId);

        var result = volunteer.Value.DeletePetSoft(petId);
        if (result.IsFailure)
            return result.Errors;
        
        _logger.LogInformation("Soft deleted pet with id {petId} from volunteer with id {volunteerId}",
            petId.Id, volunteerId.Id);

        await _unitOfWork.SaveChanges(cancellationToken);

        return petId;
    }
}