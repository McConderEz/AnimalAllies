using AnimalAllies.Application.Database;
using AnimalAllies.Application.FileProvider;
using AnimalAllies.Application.Providers;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Models.Common;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using Microsoft.Extensions.Logging;


namespace AnimalAllies.Application.Features.Volunteer.AddPetPhoto;

public class AddPetPhotosHandler
{
    private const string BUCKE_NAME = "photos";
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<AddPetPhotosHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddPetPhotosHandler(
        IFileProvider fileProvider,
        IVolunteerRepository volunteerRepository,
        ILogger<AddPetPhotosHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _fileProvider = fileProvider;
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    

    public async Task<Result<Guid>> Handle(
        AddPetPhotosCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {

            var volunteerResult = await _volunteerRepository.GetById(
                VolunteerId.Create(command.VolunteerId), cancellationToken);

            if (volunteerResult.IsFailure)
                return volunteerResult.Error;

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
                    return filePath.Error;

                var fileContent = new FileData(file.Content, filePath.Value, BUCKE_NAME);

                filesData.Add(fileContent);
            }

            var photos = filesData
                .Select(f => new PetPhoto(f.FilePath, false))
                .ToList();
            
            var petPhotoList = new PetPhotoDetails(photos);

            pet.Value.AddPhotos(petPhotoList);

            await _unitOfWork.SaveChanges(cancellationToken);
            
            var uploadResult = await _fileProvider.UploadFiles(filesData, cancellationToken);

            if (uploadResult.IsFailure)
                return uploadResult.Error;
            
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