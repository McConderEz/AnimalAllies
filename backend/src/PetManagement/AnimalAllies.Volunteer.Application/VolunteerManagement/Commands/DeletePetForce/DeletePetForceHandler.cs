using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.Database;
using AnimalAllies.Volunteer.Application.Providers;
using AnimalAllies.Volunteer.Application.Repository;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.DeletePetForce;

public class DeletePetForceHandler: ICommandHandler<DeletePetForceCommand, PetId>
{
    private const string BUCKET_NAME = "photos";
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePetForceHandler> _logger;
    private readonly IValidator<DeletePetForceCommand> _validator;
    private readonly IFileProvider _fileProvider;
    
    public DeletePetForceHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeletePetForceHandler> logger,
        IValidator<DeletePetForceCommand> validator,
        IUnitOfWork unitOfWork,
        IFileProvider fileProvider)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _fileProvider = fileProvider;
    }
    
    
    public async Task<Result<PetId>> Handle(DeletePetForceCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteer = await _volunteerRepository.GetById(volunteerId, cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Errors;

        var petId = PetId.Create(command.PetId);
        var pet = volunteer.Value.GetPetById(petId);
        if(pet.IsFailure)
            return pet.Errors;

        var result = volunteer.Value.DeletePetForce(petId);
        if (result.IsFailure)
            return result.Errors;
        
        var petPreviousPhotos = pet.Value.PetPhotoDetails
            .Select(f => new FileProvider.FileInfo(f.Path, BUCKET_NAME)).ToList();
            
        if(petPreviousPhotos.Any())
            petPreviousPhotos.ForEach(f => _fileProvider.RemoveFile(f, cancellationToken));
        
        _logger.LogInformation("Soft deleted pet with id {petId} from volunteer with id {volunteerId}",
            petId.Id, volunteerId.Id);

        await _unitOfWork.SaveChanges(cancellationToken);

        return petId;
    }
}