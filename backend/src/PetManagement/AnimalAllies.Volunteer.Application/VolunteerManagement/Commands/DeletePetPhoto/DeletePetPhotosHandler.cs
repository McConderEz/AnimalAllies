using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using AnimalAllies.SharedKernel.Shared.Ids;
using AnimalAllies.Volunteer.Application.Providers;
using AnimalAllies.Volunteer.Application.Repository;
using AnimalAllies.Volunteer.Contracts.Responses;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.Entities.Pet.ValueObjects;
using FileService.Communication;
using FileService.Contract.Requests;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.DeletePetPhoto;

public class DeletePetPhotosHandler : ICommandHandler<DeletePetPhotosCommand, DeletePetPhotosResponse>
{
    private const string BUCKET_NAME = "photos";
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeletePetPhotosHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeletePetPhotosCommand> _validator;
    private readonly FileHttpClient _fileHttpClient;

    public DeletePetPhotosHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<DeletePetPhotosHandler> logger,
        [FromKeyedServices(Constraints.Context.PetManagement)]IUnitOfWork unitOfWork,
        IValidator<DeletePetPhotosCommand> validator, 
        FileHttpClient fileHttpClient)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _fileHttpClient = fileHttpClient;
    }
    

    public async Task<Result<DeletePetPhotosResponse>> Handle(
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
            
            var petPreviousPhotos = pet.Value.PetPhotoDetails
                .Where(f => !command.FilePaths.Contains(f.Path.Path))
                .Select(f => new FileProvider.FileInfo(f.Path, BUCKET_NAME)).ToList();

            var request = new DeletePresignedUrlsRequest(petPreviousPhotos.Select(p =>
                new DeletePresignedUrlRequest(p.BucketName, Path.GetFileNameWithoutExtension(p.FilePath.Path),
                    Path.GetExtension(p.FilePath.Path))));

            var response = await _fileHttpClient.GetDeletePresignedUrlAsync(request, cancellationToken);

            if (response is null)
                return Errors.General.Null("response from file service");

            var filePaths = command.FilePaths.Select(f =>
                FilePath.Create(Guid.Parse(Path.GetFileNameWithoutExtension(f)), Path.GetExtension(f)).Value);
            
            pet.Value.DeletePhotos(filePaths);

            var deleteUrlResponse = new DeletePetPhotosResponse(response.DeleteUrl);

            await _unitOfWork.SaveChanges(cancellationToken);

            transaction.Commit();
            
            _logger.LogInformation("Files deleted from pet with id - {id}", petId.Id);

            return deleteUrlResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError("Can not delete photo from pet - {id} in transaction", command.PetId);
            
            transaction.Rollback();

            return Error.Failure("Can not delete photo from pet - {id}", "volunteer.pet.failure");
        }
    }
}