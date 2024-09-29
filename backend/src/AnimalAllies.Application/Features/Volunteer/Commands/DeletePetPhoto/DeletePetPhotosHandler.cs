using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Messaging;
using AnimalAllies.Application.Providers;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;
using FileInfo = AnimalAllies.Application.FileProvider.FileInfo;


namespace AnimalAllies.Application.Features.Volunteer.Commands.DeletePetPhoto;

public class DeletePetPhotosHandler : ICommandHandler<DeletePetPhotosCommand, Guid>
{
    private const string BUCKET_NAME = "photos";
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeletePetPhotosHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IValidator<DeletePetPhotosCommand> _validator;

    public DeletePetPhotosHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository,
        ILogger<DeletePetPhotosHandler> logger,
        IUnitOfWork unitOfWork,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IValidator<DeletePetPhotosCommand> validator)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _messageQueue = messageQueue;
        _validator = validator;
        _messageQueue = messageQueue;
    }
    

    public async Task<Result<Guid>> Handle(
        DeletePetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {

            var volunteerResult = await _volunteerRepository.GetById(
                VolunteerId.Create(command.VolunteerId), cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Errors;

            var petId = PetId.Create(command.PetId);

            var pet = volunteerResult.Value.GetPetById(petId);

            if (pet.IsFailure)
                return Errors.General.NotFound(petId.Id);
            
            var petPreviousPhotos = pet.Value.PetPhotoDetails!.Values
                .Select(f => new FileInfo(f.Path, BUCKET_NAME)).ToList();
            
            if(petPreviousPhotos.Any())
                 petPreviousPhotos.ForEach(f => _fileProvider.RemoveFile(f, cancellationToken));

            pet.Value.DeletePhotos();

            await _unitOfWork.SaveChanges(cancellationToken);

            transaction.Commit();
            
            _logger.LogInformation("Files deleted from pet with id - {id}", petId.Id);

            return pet.Value.Id.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError("Can not delete photo from pet - {id} in transaction", command.PetId);
            
            transaction.Rollback();

            return Error.Failure("Can not delete photo from pet - {id}", "volunteer.pet.failure");
        }
    }
}