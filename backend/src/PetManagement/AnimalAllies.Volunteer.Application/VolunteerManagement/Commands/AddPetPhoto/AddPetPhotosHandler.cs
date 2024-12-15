using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.Core.Messaging;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.FileProvider;
using AnimalAllies.Volunteer.Application.Providers;
using AnimalAllies.Volunteer.Application.Repository;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FileInfo = AnimalAllies.Volunteer.Application.FileProvider.FileInfo;


namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.AddPetPhoto;

public class AddPetPhotosHandler : ICommandHandler<AddPetPhotosCommand, Guid>
{
    private const string BUCKET_NAME = "photos";
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<AddPetPhotosHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IValidator<AddPetPhotosCommand> _validator;

    public AddPetPhotosHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository,
        ILogger<AddPetPhotosHandler> logger,
        [FromKeyedServices(Constraints.Context.PetManagement)]IUnitOfWork unitOfWork,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IValidator<AddPetPhotosCommand> validator)
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
        AddPetPhotosCommand command,
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

            List<FileData> filesData = [];
            foreach (var file in command.Photos)
            {
                var extension = Path.GetExtension(file.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);

                if (filePath.IsFailure)
                    return filePath.Errors;

                var fileContent = new FileData(file.Content,new FileInfo(filePath.Value, BUCKET_NAME));

                filesData.Add(fileContent);
            }

            var photos = filesData
                .Select(f => new PetPhoto(f.FileInfo.FilePath, false))
                .ToList();
            
            var petPhotoList = new ValueObjectList<PetPhoto>(photos);

            pet.Value.AddPhotos(petPhotoList);

            await _unitOfWork.SaveChanges(cancellationToken);
            
            var uploadResult = await _fileProvider.UploadFiles(filesData, cancellationToken);
            if (uploadResult.IsFailure)
            {
                await _messageQueue.WriteAsync(filesData.Select(f => f.FileInfo), cancellationToken);
                    
                return uploadResult.Errors;
            }

            transaction.Commit();
            
            _logger.LogInformation("Files uploaded to pet with id - {id}", petId.Id);

            return pet.Value.Id.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError("Can not add photo to pet - {id} in transaction", command.PetId);
            
            transaction.Rollback();

            return Error.Failure("Can not add photo to pet - {id}", "volunteer.pet.failure");
        }
    }
}