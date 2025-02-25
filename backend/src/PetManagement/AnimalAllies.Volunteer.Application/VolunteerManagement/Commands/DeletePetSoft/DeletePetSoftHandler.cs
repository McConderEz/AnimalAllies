﻿using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.DeletePetSoft;

public class DeletePetSoftHandler: ICommandHandler<DeletePetSoftCommand, PetId>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePetSoftHandler> _logger;
    private readonly IValidator<DeletePetSoftCommand> _validator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeletePetSoftHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeletePetSoftHandler> logger,
        IValidator<DeletePetSoftCommand> validator,
        [FromKeyedServices(Constraints.Context.PetManagement)]IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
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

        var result = volunteer.Value.DeletePetSoft(petId, _dateTimeProvider.UtcNow);
        if (result.IsFailure)
            return result.Errors;
        
        _logger.LogInformation("Soft deleted pet with id {petId} from volunteer with id {volunteerId}",
            petId.Id, volunteerId.Id);

        await _unitOfWork.SaveChanges(cancellationToken);

        return petId;
    }
}