using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.Database;
using AnimalAllies.Volunteer.Application.Repository;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.SetMainPhotoOfPet;

public class SetMainPhotoOfPetHandler : ICommandHandler<SetMainPhotoOfPetCommand, PetId>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SetMainPhotoOfPetHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<SetMainPhotoOfPetCommand> _validator;

    public SetMainPhotoOfPetHandler(
        IUnitOfWork unitOfWork,
        ILogger<SetMainPhotoOfPetHandler> logger,
        IValidator<SetMainPhotoOfPetCommand> validator,
        IVolunteerRepository volunteerRepository)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<PetId>> Handle(SetMainPhotoOfPetCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteer = await _volunteerRepository.GetById(volunteerId);
        if (volunteer.IsFailure)
            return volunteer.Errors;

        var petId = PetId.Create(command.PetId);

        var filePath = FilePath.Create(command.Path);
        if (filePath.IsFailure)
            return filePath.Errors;

        var petPhoto = new PetPhoto(filePath.Value, true);

        var result = volunteer.Value.SetMainPhotoOfPet(petId, petPhoto);
        if (result.IsFailure)
            return result.Errors;

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("set main photo with path {path} to pet with id {petId}",
            command.Path, command.PetId);

        return petId;
    }
}